using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Transform enemyTransform;
    private string enemyTag;
    private Animator animator;
    [SerializeField]
    private string tagCol;
    [SerializeField]
    private string enemyTagCol;

    [SerializeField]
    private float lookOffset; // Depends on the idle animation

    public CharacterStats characterStats;
    private InputHandler inputHandler;
    private Rigidbody rigidBody;
    private bool isCollidingWithEnemy = false;

    [SerializeField]
    private bool isInAir = false;
    [SerializeField]
    private float grounCheckDistance = 0.2f, jumpPower = 5f, leadPower = 2f;

    [SerializeField]
    public float pickUpRadius = 0.5f;
    [SerializeField]
    private int throwForce = 300;
    public GameObject pickedUpObject = null;

    [SerializeField]
    private float handColliderRadius = 0.1f;
    [SerializeField]
    private float footColliderRadius = 0.2f;

    private AudioSource audioSource;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        inputHandler = GetComponent<InputHandler>();
        audioSource = GetComponent<AudioSource>();
        enemyTag = TagsTypeString.Player2.ToString();
        if (gameObject.tag == TagsTypeString.Player2.ToString())
        {
            enemyTag = TagsTypeString.Player1.ToString();
            var newBackward = inputHandler.forward;
            inputHandler.forward = inputHandler.backward;
            inputHandler.backward = newBackward;
        }
        enemyTransform = GameObject.FindGameObjectWithTag(enemyTag).transform;
        tagCol = gameObject.tag == TagsTypeString.Player1.ToString() ? TagsTypeString.P1Col.ToString() : TagsTypeString.P2Col.ToString();
        enemyTagCol = gameObject.tag == TagsTypeString.Player1.ToString() ? TagsTypeString.P2Col.ToString() : TagsTypeString.P1Col.ToString();
        SetupGameObjects();
    }

    void SetupGameObjects()
    {
        foreach (var childAnimator in GetComponentsInChildren<Animator>())
        {
            if (animator != childAnimator)
            {
                animator.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break;
            }
        }
        animator.applyRootMotion = true;

        var handBones = new HumanBodyBones[2] { HumanBodyBones.LeftHand, HumanBodyBones.RightHand };
        var footBones = new HumanBodyBones[2] { HumanBodyBones.LeftFoot, HumanBodyBones.RightFoot };
        for (int i = 0; i < 2; i++)
        {
            var handBone = handBones[i];
            var footBone = footBones[i];
            var colliderObject = new GameObject("handcol", typeof(SphereCollider));
            colliderObject.tag = tagCol;
            colliderObject.GetComponent<SphereCollider>().radius = handColliderRadius;
            colliderObject.GetComponent<SphereCollider>().isTrigger = true;
            colliderObject.transform.parent = animator.GetBoneTransform(handBone);
            colliderObject.transform.localPosition = new Vector3(0, 0, 0);
            var colliderObject2 = new GameObject("footcol", typeof(SphereCollider));
            colliderObject2.tag = tagCol;
            colliderObject2.GetComponent<SphereCollider>().radius = footColliderRadius;
            colliderObject2.GetComponent<SphereCollider>().isTrigger = true;
            colliderObject2.transform.parent = animator.GetBoneTransform(footBone);
            colliderObject2.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    void Update()
    {
        inputHandler.SetupMovementInputArray();
        var blockPressed = inputHandler.MovementInputArray[(int)InputType.Block];
        animator.SetBool(AnimatorStateTypeString.Block.ToString(), blockPressed);
        isCollidingWithEnemy = false;
        CheckGroundStatus();
        if (characterStats.Health <= 0)
        {
            HandleAudio(AudioType.Die);
            animator.SetBool(AnimatorStateTypeString.Dead.ToString(), true);
            return;
        }
        if (inputHandler.SetupAttackInputArray())
        {
            HandleAttack();
        }
        if (isInAir || animator.GetInteger(AnimatorStateTypeString.Movement.ToString()) == (int)MovementType.JumpUp)
        {
            return;
        }
        animator.applyRootMotion = true;
        HandleMovement();
    }

    private void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + (Vector3.up * 0.2f), Vector3.down, out hitInfo, grounCheckDistance))
        {
            isInAir = false;
        }
        Debug.DrawRay(transform.position + (Vector3.up * 0.2f), Vector3.down, Color.yellow, Time.deltaTime);
    }

    IEnumerator HandlePickUp(AttackStats attack)
    {
        var colliders = Physics.OverlapSphere(transform.position, pickUpRadius);

        Collider to_pickUpObejct = null;
        foreach (var collider in colliders)
        {
            if (collider.gameObject.tag == TagsTypeString.Pickable.ToString())
            {
                to_pickUpObejct = collider;
                break;
            }
        }
        if (to_pickUpObejct == null) { yield return 0; }
        pickedUpObject = to_pickUpObejct.gameObject;
        pickedUpObject.GetComponent<BoxCollider>().isTrigger = true;
        pickedUpObject.GetComponent<Rigidbody>().isKinematic = true;
        pickedUpObject.tag = gameObject.tag;

        yield return new WaitForSeconds(0.9f);

        pickedUpObject.transform.parent = animator.GetBoneTransform(HumanBodyBones.RightHand);
        pickedUpObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    IEnumerator HandleThrow(AttackStats attack)
    {
        yield return new WaitForSeconds(0.55f);
        pickedUpObject.transform.parent = null;
        pickedUpObject.tag = TagsTypeString.Thrown.ToString();

        //add force
        var throwDir = enemyTransform.position - transform.position;
        pickedUpObject.GetComponent<Rigidbody>().isKinematic = false;
        pickedUpObject.GetComponent<BoxCollider>().isTrigger = false;
        pickedUpObject.GetComponent<Rigidbody>().AddForce(throwDir.normalized * throwForce);
        pickedUpObject.tag = TagsTypeString.Pickable.ToString();
        pickedUpObject = null;
    }

    void HandleAttack()
    {
        foreach (var attackObject in characterStats.attacks)
        {
            var attack = attackObject.GetComponent<AttackStats>();
            if (attack.IsActionRequired(inputHandler) && (characterStats.Power >= (int)attack.attackPowerType || attack.attackPowerType == AttackPowerType.PowerUp))
            {
                var isCroutch = animator.GetBool(AnimatorStateTypeString.Croutch.ToString());
                if (attack.attackType == AttackType.Throw && pickedUpObject == null)
                {
                    continue;
                }
                if (attack.attackType == AttackType.Throw && pickedUpObject != null && !isCroutch)
                {
                    StartCoroutine("HandleThrow", attack);
                }
                else if (attack.attackType == AttackType.PickUp && pickedUpObject == null && !isCroutch)
                {
                    StartCoroutine("HandlePickUp", attack);
                }
                HandleAudio(AudioType.Attack);
                characterStats.Power -= (int)attack.attackPowerType;
                animator.SetInteger(AnimatorStateTypeString.AttackNum.ToString(), (int)attack.attackType);
                animator.SetTrigger(AnimatorStateTypeString.Attack.ToString());
                break;
            }
        }
    }

    void HandleMovement()
    {
        var croutch = inputHandler.MovementInputArray[(int)InputType.Croutch];
        var movementString = AnimatorStateTypeString.Movement.ToString();
        if (inputHandler.MovementInputArray[(int)InputType.In])
        {
            if (!croutch)
            {
                animator.SetInteger(movementString, (int)MovementType.MoveIn);
            }
            else
            {
                animator.SetInteger(movementString, (int)MovementType.RollRight);
            }
            return;
        }
        if (inputHandler.MovementInputArray[(int)InputType.Out])
        {
            if (!croutch)
            {
                animator.SetInteger(movementString, (int)MovementType.MoveOut);
            }
            else
            {
                animator.SetInteger(movementString, (int)MovementType.RollLeft);
            }
            return;
        }
        var forward = inputHandler.MovementInputArray[(int)InputType.Forward];
        var backward = inputHandler.MovementInputArray[(int)InputType.Backward];
        var jump = inputHandler.MovementInputArray[(int)InputType.Jump];
        if (forward || backward || jump)
        {
            if (jump)
            {
                isInAir = true;
                animator.CrossFade("Idle", 0f);
                animator.applyRootMotion = false;
                animator.SetInteger(movementString, (int)MovementType.JumpUp);
                var jumpDir = (enemyTransform.position - transform.position).normalized * (forward ? leadPower : -leadPower);
                rigidBody.velocity = new Vector3(jumpDir.x, jumpPower, jumpDir.z);
                return;
            }
            if (forward)
            {
                if (!croutch)
                {
                    animator.SetInteger(movementString, (int)MovementType.MoveForward);
                }
                else
                {
                    animator.SetInteger(movementString, (int)MovementType.RollForward);
                }
                return;
            }
            if (backward)
            {
                if (!croutch)
                {
                    animator.SetInteger(movementString, (int)MovementType.MoveBackward);
                }
                else
                {
                    animator.SetInteger(movementString, (int)MovementType.RollBack);
                }
                return;
            }
        }
        animator.SetBool(AnimatorStateTypeString.Croutch.ToString(), croutch);
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == TagsTypeString.Thrown.ToString() && collider.isTrigger == false)
        {
            HandlePickableHit(collider);
        }
        else if (isCollidingWithEnemy && collider.tag == enemyTag)
        {
            return;
        }
        else if (collider.tag == enemyTagCol)
        {
            isCollidingWithEnemy = true;
            HandleHit(collider);
        }
    }

    void HandlePickableHit(Collider collider)
    {
        HandleAudio(AudioType.Hit);
        animator.CrossFade("Idle", 0f);
        animator.SetInteger(AnimatorStateTypeString.Hit.ToString(), (int)HitType.Middle);
        Destroy(collider.gameObject);
        characterStats.Health -= (int)AttackPowerType.Medium;
    }

    void HandleHit(Collider collider)
    {
        int attackNum = collider.GetComponentInParent<Animator>().GetInteger(AnimatorStateTypeString.AttackNum.ToString());
        foreach (var attackObject in collider.GetComponentInParent<Character>().characterStats.attacks)
        {
            var attack = attackObject.GetComponent<AttackStats>();
            var enemyAnimator = collider.GetComponentInParent<Animator>();
            if (attack.attackType == (AttackType)attackNum)
            {
                if (
                    animator.GetInteger(AnimatorStateTypeString.Hit.ToString()) == (int)HitType.None
                    && enemyAnimator.GetInteger(AnimatorStateTypeString.AttackNum.ToString()) != (int)AttackType.None
                    && enemyAnimator.GetInteger(AnimatorStateTypeString.Movement.ToString()) == (int)MovementType.None
                    )
                {
                    if (pickedUpObject != null)
                    {
                        pickedUpObject.transform.parent = null;
                    }
                    characterStats.Health -= (int)attack.attackPowerType;
                    HandleAudio(AudioType.Hit);
                    animator.CrossFade("Idle", 0f);
                    animator.SetInteger(AnimatorStateTypeString.Hit.ToString(), (int)attack.hitType);
                    break;
                }
            }
        }
    }

    //after animations, for camera
    void LateUpdate()
    {
        LookAtTarget();
    }

    void LookAtTarget()
    {
        //turn the complete game object
        var lookDir = enemyTransform.position - transform.position;
        lookDir.y = 0;
        var rot = Quaternion.LookRotation(lookDir) * Quaternion.Euler(0, lookOffset, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.1f);

        //turn using animators
        //var relativePos = transform.InverseTransformPoint(enemy.position);
        //animator.SetFloat("Turn", relativePos.normalized.x);
    }
    void HandleAudio(AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.Hit:
                {
                    if (animator.GetInteger(AnimatorStateTypeString.Hit.ToString()) == (int)HitType.None && !audioSource.isPlaying)
                        audioSource.PlayOneShot(characterStats.hitclips[Random.Range(0, characterStats.hitclips.Length)]);
                    break;
                }
            case AudioType.Attack:
                {
                    if (animator.GetInteger(AnimatorStateTypeString.AttackNum.ToString()) == (int)AttackType.None && !audioSource.isPlaying)
                        audioSource.PlayOneShot(characterStats.attackclips[Random.Range(0, characterStats.attackclips.Length)]);
                    break;
                }
            case AudioType.Die:
                {
                    if (!animator.GetBool(AnimatorStateTypeString.Dead.ToString()) && !audioSource.isPlaying)
                        audioSource.PlayOneShot(characterStats.dieclips[Random.Range(0, characterStats.dieclips.Length)]);
                    break;
                }
        }
    }
}