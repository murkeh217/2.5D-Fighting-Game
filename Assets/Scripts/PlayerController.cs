using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerController;

    public Animator anim;
    private float moveAcceleration = 10f;

    public float jumpHeight;

    private float distance;
    public Transform target;

    Rigidbody rb;

    //public bool trigger;

    readonly int hashPunchAttack = Animator.StringToHash("PunchAttack");
    readonly int hashKickAttack = Animator.StringToHash("KickAttack");

    readonly int hashNextMove = Animator.StringToHash("NextMove");


    public Animation cylinderHit;

    private void Start()
    {
        anim.updateMode = AnimatorUpdateMode.AnimatePhysics;

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

        distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < 20)
        {
            var targetPosition = target.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);
        }

    }

    public void NextMove() // called by animation event
    {
        anim.SetTrigger(hashNextMove);
    }



    void OnTriggerStay(Collider other)
    {
        //trigger = false;
    }


    void Awake()
    {
        playerController = this;
        rb = GetComponent<Rigidbody>();

    }


    void FixedUpdate()
    {
        //trigger = true;

        float horizontal = Input.GetAxis("Horizontal"); //moves player horizontally
        anim.SetFloat("horizontal", horizontal);
        rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.right * horizontal * Time.deltaTime, moveAcceleration * Time.deltaTime);

        float step = Input.GetAxis("Z-Axis"); //side stepping code
        anim.SetFloat("step", step);
        rb.velocity = new Vector3(0, 0, step * moveAcceleration * Time.deltaTime);
    }

}

