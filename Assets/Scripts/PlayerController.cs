using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerController;

    public Animator anim;
    public float moveSpeed = 5f;     // Set this higher to make player faster
    public float jumpHeight;

    public Transform target;
    private float distance;

    private Rigidbody rb;

    readonly int hashPunchAttack = Animator.StringToHash("PunchAttack");
    readonly int hashKickAttack = Animator.StringToHash("KickAttack");
    readonly int hashNextMove = Animator.StringToHash("NextMove");

    public Animation cylinderHit;

    private void Start()
    {
        anim.updateMode = AnimatorUpdateMode.Fixed;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetTrigger(hashPunchAttack);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.SetTrigger(hashKickAttack);
        }

        distance = Vector3.Distance(transform.position, target.position);

        if (distance < 20)
        {
            var targetPosition = target.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);
        }
    }

    public void NextMove() // Called by animation event
    {
        anim.SetTrigger(hashNextMove);
    }

    private void Awake()
    {
        playerController = this;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Z-Axis"); // your side-step input (could change to "Vertical")

        anim.SetFloat("horizontal", horizontal);
        anim.SetFloat("step", vertical);

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        rb.linearVelocity = moveDirection * moveSpeed;
    }
}
