using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject jotaro;
    public Animator anim;

    private float distance;
    public Transform target;

    private float moveAcceleration = 10f;

    Rigidbody rb;

    float horizontal;

    readonly int bpl = Animator.StringToHash("Attack 0");
    readonly int bpr = Animator.StringToHash("Attack 1");
    readonly int hpl = Animator.StringToHash("Attack 2");
    readonly int hpr = Animator.StringToHash("Attack 3");

    IEnumerator DelayAttack()
    {
        yield return new WaitForSecondsRealtime(30f);
        anim.Play(bpl);
        anim.Play(bpr);
    }

    void Start()
    {
        anim.updateMode = AnimatorUpdateMode.AnimatePhysics;

        anim = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();


        anim.SetInteger(bpl, 0);
        anim.SetInteger(bpr, 1);
        anim.SetInteger(hpl, 2);
        anim.SetInteger(hpr, 3);


    }

    // Update is called once per frame
    void Update()
    {
        //lock on code
        distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < 4 && distance > 2)
        {
            var targetPosition = target.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);
            
            //move
            horizontal = 0.3f;
            anim.SetFloat("horizontal", horizontal);
            rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.right * horizontal * Time.deltaTime, moveAcceleration * Time.deltaTime);
            

        }

        else if (distance < 1)
        {
            //anim.Play(bpl);
            StartCoroutine(DelayAttack());
            anim.StopPlayback();
        }
    }

    void FixedUpdate() 
    {

    }

    void ComboAttack()
    {
        //if (distance < 2)
        //{
            switch (Random.Range(0, 4))
            {
                case 1:
                    StartCoroutine(DelayAttack());

                    anim.Play(bpl);
                    break;
                case 2:
                    StartCoroutine(DelayAttack());

                    anim.Play(bpr);
                    break;
                case 3:
                    StartCoroutine(DelayAttack());

                    anim.Play(hpl);
                    break;
                case 4:
                    StartCoroutine(DelayAttack());

                    anim.Play(hpr);
                    break;

                case 0:
                    horizontal = -0.3f;
                    anim.SetFloat("horizontal", horizontal);
                    rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.right * horizontal * Time.deltaTime, moveAcceleration * Time.deltaTime);
                    break;
            }
        //}
    }

    void OnTriggerEnter(Collider collider)
    {
        horizontal = 0;
        anim.SetFloat("horizontal", horizontal);
        rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.right * horizontal * Time.deltaTime, moveAcceleration * Time.deltaTime);

        //if (collider.CompareTag("Player"))
        //{
        
        //}
    }
    void OnTriggerExit(Collider collider)
    {
       anim.StopPlayback();
    }
}
