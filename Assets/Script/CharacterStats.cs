using UnityEngine;
using System.Collections.Generic;

public class CharacterStats : MonoBehaviour
{
    public List<GameObject> attacks;
    public List<GameObject> movements;

    [SerializeField]
    private float health = 100;
    [SerializeField]
    private float power = 100;

    [SerializeField]
    public AudioClip[] attackclips, hitclips, dieclips;

    public float Health
    {
        get
        {
            return health;
        }

        set
        {
            if (value < 0)
            {
                value = 0;
            }
            health = value;
        }
    }

    public float Power
    {
        get
        {
            return power;
        }

        set
        {
            if (value > 100)
            {
                value = 100;
            }
            if (value < (int)AttackPowerType.High)
            {
                value = (int)AttackPowerType.High;
            }
            power = value;
        }
    }

    void Start()
    {
        attacks.Sort(new InputLengthComparer());
    }

    void LateUpdate()
    {
        Power += 0.001f;
    }
}