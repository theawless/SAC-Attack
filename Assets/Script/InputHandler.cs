using System;
using System.Linq;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public KeyCode jump;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode block;
    public KeyCode rightKick;
    public KeyCode leftKick;
    public KeyCode rightPunch;
    public KeyCode leftPunch;
    public KeyCode pickUp;

    private int inputFrameNumber = 1;
    private int inputFrameSkip = 20;
    public bool[] AttackInputArray { get; private set; }
    public bool[] MovementInputArray { get; private set; }

    public InputHandler() : base()
    {
        AttackInputArray = new bool[Enum.GetValues(typeof(InputType)).Cast<int>().Last() + 1];
        MovementInputArray = new bool[Enum.GetValues(typeof(InputType)).Cast<int>().Last() + 1];
    }

    protected void AttackInputArrayReset()
    {
        foreach (InputType input in Enum.GetValues(typeof(InputType)))
        {
            AttackInputArray[(int)input] = false;
        }
    }

    public virtual void SetupMovementInputArray()
    {
        foreach (InputType input in Enum.GetValues(typeof(InputType)))
        {
            MovementInputArray[(int)input] = GetInput(input);
        }
    }

    public bool SetupAttackInputArray()
    {
        if (inputFrameNumber == 1)
        {
            AttackInputArrayReset();
        }
        if (inputFrameNumber != inputFrameSkip)
        {
            foreach (InputType input in Enum.GetValues(typeof(InputType)))
            {
                AttackInputArray[(int)input] = AttackInputArray[(int)input] || GetInput(input);
            }
            inputFrameNumber++;
        }
        else if (inputFrameNumber == inputFrameSkip)
        {
            inputFrameNumber = 1;
            return true;
        }
        return false;
    }

    public virtual bool GetInput(InputType input)
    {
        switch (input)
        {
            case InputType.Jump:
                {
                    return Input.GetKey(jump);
                }
            case InputType.Croutch:
                {
                    return Input.GetKey(down);
                }
            case InputType.Out:
                {
                    return Input.GetKey(left);
                }
            case InputType.In:
                {
                    return Input.GetKey(right);
                }
            case InputType.Forward:
                {
                    return Input.GetKey(forward);
                }
            case InputType.Backward:
                {
                    return Input.GetKey(backward);
                }
            case InputType.Block:
                {
                    return Input.GetKey(block);
                }
            case InputType.AttackOne:
                {
                    return Input.GetKey(rightKick);
                }
            case InputType.AttackTwo:
                {
                    return Input.GetKey(leftKick);
                }
            case InputType.AttackThree:
                {
                    return Input.GetKey(rightPunch);
                }
            case InputType.AttackFour:
                {
                    return Input.GetKey(leftPunch);
                }
            case InputType.PickUp:
                {
                    return Input.GetKey(pickUp);
                }
            default:
                {
                    Debug.Log("Weird input request");
                    return false;
                }
        }
    }
}
