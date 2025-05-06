using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject jotaro;
    public Animator anim;

    private float distance;
    public Transform target;

    private float moveSpeed = 2f;  // Adjust this to make the enemy slower
    private bool isAttacking = false;  // To prevent multiple attacks at once

    Rigidbody rb;

    float horizontal;

    readonly int bpl = Animator.StringToHash("Attack 0");
    readonly int bpr = Animator.StringToHash("Attack 1");
    readonly int hpl = Animator.StringToHash("Attack 2");
    readonly int hpr = Animator.StringToHash("Attack 3");

    IEnumerator DelayAttack()
    {
        // Wait for the attack animation to complete before returning to movement
        yield return new WaitForSeconds(1f);  // Adjust this value to match your attack animation length
        isAttacking = false;  // Allow the enemy to move back
    }

    void Start()
    {
        anim.updateMode = AnimatorUpdateMode.Fixed;
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
        distance = Vector3.Distance(transform.position, target.position);

        if (distance < 4 && distance > 2 && !isAttacking)
        {
            var targetPosition = target.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);

            // Move towards the target at the reduced speed
            horizontal = 0.3f;
            anim.SetFloat("horizontal", horizontal);
            rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, Vector3.right * horizontal * moveSpeed * Time.deltaTime, moveSpeed * Time.deltaTime);
        }

        else if (distance < 1 && !isAttacking)
        {
            // Start attack sequence
            isAttacking = true;
            StartCoroutine(DelayAttack());
            ComboAttack();  // Initiate the combo attack logic
        }
    }

    void ComboAttack()
    {
        // Attack combo logic
        int attackChoice = Random.Range(0, 4);
        switch (attackChoice)
        {
            case 1:
                anim.Play(bpl);
                break;
            case 2:
                anim.Play(bpr);
                break;
            case 3:
                anim.Play(hpl);
                break;
            case 4:
                anim.Play(hpr);
                break;
            case 0:
                horizontal = -0.3f;
                anim.SetFloat("horizontal", horizontal);
                rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, Vector3.right * horizontal * moveSpeed * Time.deltaTime, moveSpeed * Time.deltaTime);
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        // Reset movement on trigger enter
        horizontal = 0;
        anim.SetFloat("horizontal", horizontal);
        rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, Vector3.right * horizontal * moveSpeed * Time.deltaTime, moveSpeed * Time.deltaTime);
    }

    void OnTriggerExit(Collider collider)
    {
        // Stop animation and reset state when exiting trigger
        anim.StopPlayback();
    }
}
