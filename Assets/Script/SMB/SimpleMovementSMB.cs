using UnityEngine;
using System.Collections;
using System;

public class SimpleMovementSMB : ExitableSMB
{
    protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(AnimatorStateTypeString.Movement.ToString(), (int)MovementType.None);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(AnimatorStateTypeString.Movement.ToString(), (int)MovementType.None);
    }
}