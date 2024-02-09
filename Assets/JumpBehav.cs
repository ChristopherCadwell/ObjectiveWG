using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBehav : StateMachineBehaviour
{
    private float speedParameter = 0.0001f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (CombatTest.PlayerCombatInstance.isAttacking && !animator.GetBool("Grounded"))
        {
            CombatTest.PlayerCombatInstance.anim.Play("JumpPunch1");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!CombatTest.PlayerCombatInstance.isAttacking && !animator.GetBool("Grounded"))
        {
            if (animator.GetFloat("Speed") > speedParameter)
            {
                CombatTest.PlayerCombatInstance.anim.Play("Jump A");
            }
            else if (animator.GetFloat("Speed") < speedParameter)
            {
                CombatTest.PlayerCombatInstance.anim.Play("Jump B");
            }
        }
        else
        {
            CombatTest.PlayerCombatInstance.isAttacking = false;
        }

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}