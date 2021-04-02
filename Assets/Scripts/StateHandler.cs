using UnityEngine;


[SharedBetweenAnimators]
public class StateHandler : StateMachineBehaviour
{

    readonly int hashPunchAttack = Animator.StringToHash("PunchAttack");
    readonly int hashKickAttack = Animator.StringToHash("KickAttack");
    readonly int hashNextMove = Animator.StringToHash("NextMove");

    readonly int bpl = Animator.StringToHash("Body_Left_Punch");
    readonly int bpr = Animator.StringToHash("Body_Right_Punch");
    readonly int bkl = Animator.StringToHash("Body_Left_Kick");
    public int actionState = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actionState == 0)
        {
            ResetTriggers(animator);
        }

        animator.SetInteger(bpl, 0);
        animator.SetInteger(bpr, 1);
        animator.SetInteger(bkl, 2);
        // =========================================

        // PlayerController playerController = animator.GetComponent<PlayerController>();
        HurtController.Instance.totalHits = 0;
    }

    private void ResetTriggers(Animator anim) // we want to reset triggers to prevent inputs from registering when the player returns back to idle state
    {
        anim.ResetTrigger(hashPunchAttack);
        anim.ResetTrigger(hashKickAttack);
        anim.ResetTrigger(hashNextMove);
    }
}

