using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackStats : MonoBehaviour
{
    public AttackType attackType;
    public HitType hitType;
    public AttackPowerType attackPowerType;

    [SerializeField]
    protected InputType key1, key2, key3;
    [SerializeField]
    public InputLengthType length;

    public void MakeActionRequired(InputHandler inputHandler)
    {
        var inputArray = inputHandler.AttackInputArray;
        inputArray[(int)key1] = true;
        inputArray[(int)key2] = true;
        inputArray[(int)key3] = true;
    }

    public bool IsActionRequired(InputHandler inputHandler)
    {
        var inputArray = inputHandler.AttackInputArray;
        if (attackType == AttackType.Super3 || attackType == AttackType.Super4)
        {
            return false;
        }
        return inputArray[(int)key1] && inputArray[(int)key2] && inputArray[(int)key3];
    }
}

public class InputLengthComparer : IComparer<GameObject>
{
    public int Compare(GameObject x, GameObject y)
    {
        var firstLength = (int)x.GetComponent<AttackStats>().length;
        var secondLength = (int)y.GetComponent<AttackStats>().length;
        if (firstLength > secondLength)
        {
            return -1;
        }
        else if (firstLength == secondLength)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
}
