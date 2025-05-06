using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HurtController : GameManager<HurtController>
{
    //public static HurtController hurtController;


    public Animator targetAnim;
    public Animator fighterAnim;

    public string setState;
    public string setHit;


    public int totalHits;

    public Text p1;
    public Text target;
    public Text p1Hits;
    public Text p2Hits;
    public Text title;

    public CinemachineVirtualCamera vcam = null;

    public GameObject targetPlayer;
    //public GameObject yellowPlayer;


    public Material hurtMat;
    public Material dioMat;
    public Material jotaroMat;

    private float jotaroHealth;
    private float dioHealth;

    public Image p1Health;
    public Image p2Health;

    public Color critical;

    public Text timer;
    public float timeLeft = 100f;
    private bool condition = true;

    private float distance;
    public Transform targetTransform;

    public bool trigger;

    public string targetChoose;


    IEnumerator HitStop()
    {
        if (targetChoose == "Jotaro")
        {
            Time.timeScale = 0f;
            //targetPlayer.GetComponentInChildren<SkinnedMeshRenderer>().material = hurtMat;
            yield return new WaitForSecondsRealtime(0.2f);
            //targetPlayer.GetComponentInChildren<SkinnedMeshRenderer>().material = jotaroMat;
            Time.timeScale = 1f;
        }
        else {
            Time.timeScale = 0f;
            //targetPlayer.GetComponentInChildren<SkinnedMeshRenderer>().material = hurtMat;
            yield return new WaitForSecondsRealtime(0.2f);
            //targetPlayer.GetComponentInChildren<SkinnedMeshRenderer>().material = dioMat;
            Time.timeScale = 1f;
        }
    }


    IEnumerator TextState()
    {
        if (targetChoose == "Jotaro")
        {
            //Time.timeScale = 0f;
            title.enabled = true;
            p1Hits.enabled = true;
            yield return new WaitForSecondsRealtime(2f);
            title.enabled = false;
            p1Hits.enabled = false;
            if (targetAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                totalHits = 0;
            }
            //Time.timeScale = 1f;
        }
        else
        {  //Time.timeScale = 0f;
            title.enabled = true;
            p2Hits.enabled = true;
            yield return new WaitForSecondsRealtime(2f);
            title.enabled = false;
            p2Hits.enabled = false;
            if (targetAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                totalHits = 0;
            }
            //Time.timeScale = 1f;}
        }
    }
        void Start()
    {

    }
    void Update()
    {
        /*distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < 20)
        {
            var targetPosition = target.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);
        }*/

        if (targetChoose == "Jotaro")
        {
            if (condition == true)
            {
                timeLeft -= Time.deltaTime;
                timer.text = (timeLeft).ToString("0");

                if (timeLeft <= 0)
                {
                    if (p2Health.fillAmount < p1Health.fillAmount)
                    {
                        vcam.enabled = true;
                        targetAnim.Play("KO");
                        title.text = "Dio WINS";
                        fighterAnim.Play("Win");
                    }
                    condition = false;

                }
            }
        }
        else
        {
            if (condition == true)
            {
                timeLeft -= Time.deltaTime;
                timer.text = (timeLeft).ToString("0");

                if (timeLeft <= 0)
                {
                    if (p1Health.fillAmount < p2Health.fillAmount)
                    {
                        vcam.enabled = true;
                        targetAnim.Play("KO");
                        title.text = "Jotaro WINS";
                        fighterAnim.Play("Win");
                    }
                    condition = false;

                }
            }
        }
        



    }

    //needed to be overrided
    override public void Awake()
    {
        //hurtController = this;
        vcam.enabled = false;
        jotaroHealth = p2Health.fillAmount;
        dioHealth = p1Health.fillAmount;
    }

    void FixedUpdate()
    {
        trigger = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        // Jotaro is the enemy
        if (targetChoose == "Jotaro")
        {
            // Check if the hit is from Dio's Hands or Legs (Player)
            if (collider.CompareTag("Hands") && Input.GetKey(KeyCode.J) || collider.CompareTag("Legs") && Input.GetKey(KeyCode.K))
            {
                switch (Mathf.Clamp(totalHits, 0, 4))
                {
                    case 1:
                        title.text = "WRYYYY";
                        p1Hits.text = "1 hit";
                        StartCoroutine(TextState());
                        break;
                    case 2:
                        title.text = "HIT";
                        p1Hits.text = "2 hits";
                        StartCoroutine(TextState());
                        break;
                    case 3:
                        title.text = "HIT";
                        p1Hits.text = "3 hits!";
                        StartCoroutine(TextState());
                        break;
                    case 4:
                        title.text = "HIT";
                        p1Hits.text = "MUDA MUDA";
                        StartCoroutine(TextState());
                        StartCoroutine(HitStop());
                        break;
                    case 0:
                        title.text = "You dare approach me?!";
                        break;
                }

                // Apply damage to Jotaro (enemy)
                if (targetAnim.GetCurrentAnimatorStateInfo(0).IsName(setState))
                {
                    targetAnim.Play("Head_Hurt");
                    p2Health.fillAmount -= 0.005f;  // Damage Jotaro's health
                    jotaroHealth = p2Health.fillAmount;
                    totalHits++;
                }
                else
                {
                    targetAnim.Play("Body_Hurt");
                    p2Health.fillAmount -= 0.008f;  // Damage Jotaro's health
                    jotaroHealth = p2Health.fillAmount;
                    totalHits++;
                }

                Debug.Log("Jotaro Hit!" + totalHits);

                // Update health bar color to critical if Jotaro's health is low
                if (p2Health.fillAmount < 0.3f)
                {
                    p2Health.color = critical;
                }
            }
        }
        // Dio is the player
        else
        {
            // Check if the hit is from Jotaro's Hands or Legs (Enemy)
            if (collider.CompareTag("Hands") || collider.CompareTag("Legs"))
            {
                switch (Mathf.Clamp(totalHits, 0, 4))
                {
                    case 1:
                        title.text = "Hmph";
                        p2Hits.text = "1 hit";
                        StartCoroutine(TextState());
                        break;
                    case 2:
                        title.text = "HIT";
                        p2Hits.text = "2 hits";
                        StartCoroutine(TextState());
                        break;
                    case 3:
                        title.text = "HIT";
                        p2Hits.text = "3 hits!";
                        StartCoroutine(TextState());
                        break;
                    case 4:
                        title.text = "HIT";
                        p2Hits.text = "ORA ORA";
                        StartCoroutine(TextState());
                        StartCoroutine(HitStop());
                        break;
                    case 0:
                        title.text = "You are the lowest scum";
                        break;
                }

                // Apply damage to Dio (player)
                if (targetAnim.GetCurrentAnimatorStateInfo(0).IsName(setState))
                {
                    targetAnim.Play("Head_Hurt");
                    p1Health.fillAmount -= 0.005f;  // Damage Dio's health
                    dioHealth = p1Health.fillAmount;
                    totalHits++;
                }
                else
                {
                    targetAnim.Play("Body_Hurt");
                    p1Health.fillAmount -= 0.008f;  // Damage Dio's health
                    dioHealth = p1Health.fillAmount;
                    totalHits++;
                }

                Debug.Log("Dio Hit!" + totalHits);

                // Update health bar color to critical if Dio's health is low
                if (p1Health.fillAmount < 0.3f)
                {
                    p1Health.color = critical;
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        trigger = false;
    }

    void OnTriggerExit(Collider other)
    {
        if (targetChoose == "Jotaro")
        {
            if (p2Health.fillAmount <= 0f)
            {
                vcam.enabled = true;
                targetAnim.Play("KO");
                title.text = "Dio WINS";
                fighterAnim.Play("Win");
            }
        }
        else
        {
            if (p1Health.fillAmount <= 0f)
            {
                vcam.enabled = true;
                targetAnim.Play("KO");
                title.text = "Jotaro WINS";
                fighterAnim.Play("Win");
            }
        }
    }
}
