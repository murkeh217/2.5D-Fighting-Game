using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DepthNormalsFeature : ScriptableRendererFeature
{
    class DepthNormalsPass : ScriptableRenderPass
    {
        private RTHandle destination;
        private Material depthNormalsMaterial = null;
        private FilteringSettings m_FilteringSettings;
        ShaderTagId m_ShaderTagId = new ShaderTagId("DepthOnly");

        public DepthNormalsPass(RenderQueueRange renderQueueRange, LayerMask layerMask, Material material)
        {
            m_FilteringSettings = new FilteringSettings(renderQueueRange, layerMask);
            this.depthNormalsMaterial = material;
        }

        public void Setup(RTHandle destination)
        {
            this.destination = destination;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cameraTextureDescriptor.depthBufferBits = 32;
            cameraTextureDescriptor.colorFormat = RenderTextureFormat.ARGB32;

            RenderingUtils.ReAllocateIfNeeded(ref destination, cameraTextureDescriptor, name: "_CameraDepthNormalsTexture");

            ConfigureTarget(destination);
            ConfigureClear(ClearFlag.All, Color.black);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("DepthNormals Prepass");

            using (new ProfilingScope(cmd, new ProfilingSampler("DepthNormals Prepass")))
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                var sortFlags = renderingData.cameraData.defaultOpaqueSortFlags;
                var drawSettings = CreateDrawingSettings(m_ShaderTagId, ref renderingData, sortFlags);
                drawSettings.perObjectData = PerObjectData.None;
                drawSettings.overrideMaterial = depthNormalsMaterial;

                ref CameraData cameraData = ref renderingData.cameraData;
                Camera camera = cameraData.camera;

                if (cameraData.xr.enabled)
                    context.StartMultiEye(camera);

                context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref m_FilteringSettings);

                cmd.SetGlobalTexture("_CameraDepthNormalsTexture", destination.nameID);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            // No manual release here. RTHandles are released in the feature's Dispose method.
        }
    }

    DepthNormalsPass depthNormalsPass;
    RTHandle depthNormalsTexture;
    Material depthNormalsMaterial;

    public override void Create()
    {
        depthNormalsMaterial = CoreUtils.CreateEngineMaterial("Hidden/Internal-DepthNormalsTexture");

        depthNormalsTexture = RTHandles.Alloc(
            "_CameraDepthNormalsTexture",
            name: "_CameraDepthNormalsTexture"
        );

        depthNormalsPass = new DepthNormalsPass(RenderQueueRange.opaque, -1, depthNormalsMaterial)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingPrePasses
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        depthNormalsPass.Setup(depthNormalsTexture);
        renderer.EnqueuePass(depthNormalsPass);
    }

    protected override void Dispose(bool disposing)
    {
        if (depthNormalsTexture != null)
        {
            depthNormalsTexture.Release();
            depthNormalsTexture = null;
        }
    }
}
