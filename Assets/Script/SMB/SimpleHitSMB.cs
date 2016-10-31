using UnityEngine;
using System.Collections;
using System;

public class SimpleHitSMB : ExitableSMB
{
    protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Exit");
        //animator.SetInteger(AnimatorStateTypeString.Hit.ToString(),(int)HitType.None);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("ExitO");
        animator.SetInteger(AnimatorStateTypeString.Hit.ToString(), (int)HitType.None);
        //base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
