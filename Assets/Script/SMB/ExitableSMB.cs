using UnityEngine;
using System.Collections;

public abstract class ExitableSMB : StateMachineBehaviour
{
    private float length;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        length = 0;
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        length += Time.deltaTime;
        if (length > stateInfo.length)
        {
            StateExit(animator, stateInfo, layerIndex);
        }
    }

    abstract protected void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //       animator.SetInteger(AnimatorStateTypeString.Attack.ToString(), (int)AttackType.None);
    //    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}