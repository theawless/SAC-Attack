using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AI_InputHandler : InputHandler
{
    private GameObject enemy;
    private string enemyTag;
    private CharacterStats characterStats;
    private Character character;

    private int frameNumber = 1;
    [SerializeField]
    private int frameSkip = 100;

    [SerializeField]
    private int attackDistance = 2;
    [SerializeField]
    private int moveDistance = 4;

    private bool isPickableObjectInRange = false;

    private List<AttackStats> SuperAttacks = new List<AttackStats>(2);
    private List<AttackStats> NormalAttacks = new List<AttackStats>(4);
    private AttackStats PowerUp;
    private AttackStats Throw;
    private AttackStats PickUp;

    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        character = GetComponent<Character>();
        gameObject.tag = TagsTypeString.Player2.ToString();
        enemyTag = TagsTypeString.Player1.ToString();
        enemy = GameObject.FindGameObjectWithTag(enemyTag);
        SetUpAttacks();
    }

    void SetUpAttacks()
    {
        foreach (var attackObject in characterStats.attacks)
        {
            var attack = attackObject.GetComponent<AttackStats>();
            switch (attack.attackType)
            {
                case AttackType.None:
                    {
                        continue;
                    }
                case AttackType.PowerUp:
                    {
                        PowerUp = attack; continue;
                    }
                case AttackType.PickUp:
                    {
                        PickUp = attack; continue;
                    }
                case AttackType.Throw:
                    {
                        Throw = attack; continue;
                    }
                case AttackType.Super1:
                    {
                        SuperAttacks.Add(attack); continue;
                    }
                case AttackType.Super2:
                    {
                        SuperAttacks.Add(attack); continue;
                    }
                case AttackType.Super3:
                    {
                        SuperAttacks.Add(attack); continue;
                    }
                case AttackType.Super4:
                    {
                        SuperAttacks.Add(attack); continue;
                    }
                default:
                    {
                        NormalAttacks.Add(attack); continue;
                    }
            }
        }

    }

    protected void MovementInputArrayReset()
    {
        foreach (InputType input in Enum.GetValues(typeof(InputType)))
        {
            MovementInputArray[(int)input] = false;
        }
    }
    void Update()
    {
        var enemyLineVector = transform.position - enemy.transform.position;
        var enemyDistance = enemyLineVector.magnitude;
        if (MovementInputArray[(int)InputType.Block])
        {
            return;
        }
        if (MovementInputArray[(int)InputType.Croutch])
        {
            AttackInputArrayReset();
            if (UnityEngine.Random.Range(0, 1) == 2 && enemyDistance < attackDistance)
            {
                NormalAttacks[UnityEngine.Random.Range(0, NormalAttacks.Count)].MakeActionRequired(this);
            }
            return;
        }
        MovementInputArrayReset();
        LookForPickableObjects();
        frameNumber = (frameNumber + 1) % frameSkip;
        if (enemyDistance > attackDistance)
        {
            if (isPickableObjectInRange)
            {
                AttackInputArrayReset();
                PickUp.MakeActionRequired(this);
                frameNumber = 1;
            }
            if (characterStats.Power <= (int)AttackPowerType.High)
            {
                AttackInputArrayReset();
                PowerUp.MakeActionRequired(this);
                frameNumber = 1;
            }
        }
        if (character.pickedUpObject != null)
        {
            AttackInputArrayReset();
            Throw.MakeActionRequired(this);
            frameNumber = 1;
        }
        if (frameNumber == 0)
        {
            if (enemyDistance <= attackDistance)
            {
                HandleAttackAndBlock();
            }
            else if (enemyDistance >= moveDistance)
            {
                HandleMovement(enemyLineVector);
            }
        }
    }
    public override void SetupMovementInputArray()
    {
        return;
    }

    private void LookForPickableObjects()
    {
        var colliders = Physics.OverlapSphere(transform.position, character.pickUpRadius);
        Collider to_pickUpObejct = null;
        foreach (var collider in colliders)
        {
            if (collider.gameObject.tag == TagsTypeString.Pickable.ToString())
            {
                to_pickUpObejct = collider;
                break;
            }
        }
        isPickableObjectInRange = to_pickUpObejct != null;
    }

    private void HandleMovement(Vector3 enemyLineVector)
    {
        MovementInputArrayReset();
        var probMoveRandom = UnityEngine.Random.Range(0, 4);
        if (probMoveRandom == 1)
        {
            MovementInputArray[(int)InputType.Forward] = true;
            MovementInputArray[(int)InputType.Jump] = true;
        }
        else if (probMoveRandom == 2)
        {
            MovementInputArray[(int)InputType.Croutch] = true;
            MovementInputArray[(int)InputType.Forward] = true;
            StartCoroutine("DisableCroutch");
        }
        else
        {
            MovementInputArray[(int)InputType.Forward] = true;
        }
    }

    private void HandleAttackAndBlock()
    {
        var probRange = 8;
        var probOfAttack = UnityEngine.Random.Range(0, probRange);
        AttackInputArrayReset();
        if ((probOfAttack % probRange == 0 || probOfAttack % probRange == 1 || probOfAttack % probRange == 2 || probOfAttack % probRange == 7) && characterStats.Power >= (int)AttackPowerType.Highest)
        {
            SuperAttacks[UnityEngine.Random.Range(0, SuperAttacks.Count)].MakeActionRequired(this);
        }
        else if (probOfAttack % probRange == 3)
        {
            NormalAttacks[UnityEngine.Random.Range(0, NormalAttacks.Count)].MakeActionRequired(this);
        }
        else if (probOfAttack % probRange == 4)
        {
            MovementInputArrayReset();
            MovementInputArray[(int)InputType.Block] = true;
            StartCoroutine("DisableBlock");
        }
        else if (probOfAttack % probRange == 5)
        {
            MovementInputArrayReset();
            MovementInputArray[(int)InputType.Croutch] = true;
            StartCoroutine("DisableCroutch");
        }
        else if (probOfAttack % probRange == 6 && characterStats.Health < (int)AttackPowerType.Highest + (int)AttackPowerType.High)
        {
            MovementInputArrayReset();
            var probMoveRandom = UnityEngine.Random.Range(0, 2);
            if (probMoveRandom == 0)
            {
                MovementInputArray[(int)InputType.Backward] = true;
            }
            else if (probMoveRandom == 1)
            {
                MovementInputArray[(int)InputType.In] = true;
            }
            else
            {
                MovementInputArray[(int)InputType.Out] = true;
            }
        }
    }

    IEnumerator DisableCroutch()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 4f));
        MovementInputArrayReset();
    }
    IEnumerator DisableBlock()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 3f));
        MovementInputArrayReset();
    }
}