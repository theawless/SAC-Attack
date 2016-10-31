using UnityEngine;
using System.Collections;
using System;

public class SimpleAttackSMB : ExitableSMB
{
    protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(AnimatorStateTypeString.AttackNum.ToString(), (int)AttackType.None);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(AnimatorStateTypeString.AttackNum.ToString(), (int)AttackType.None);
    }

}
