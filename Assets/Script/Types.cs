public enum InputType
{
    Jump,
    Croutch,
    Out,
    In,
    Forward,
    Backward,
    Block,
    AttackOne,
    AttackTwo,
    AttackThree,
    AttackFour,
    PickUp,
}

public enum AnimatorStateTypeString
{
    Idle,
    Movement,
    Hit,
    Block,
    Attack,
    AttackNum,
    PickUp,
    Dead,
    Stun,
    Pose,
    Croutch,
}

public enum MovementType
{
    None,
    MoveForward,
    MoveBackward,
    MoveIn,
    MoveOut,
    JumpUp,
    RollBack,
    RollForward,
    RollLeft,
    RollRight,
}

public enum AttackType
{
    None,
    LeftPunch,
    RightPunch,
    LeftKick,
    RightKick,
    PowerUp,
    Super1,
    Super2,
    Super3,
    Super4,
    Throw,
    PickUp,
}

public enum AttackPowerType
{
    None = 0,
    Lowest = 3,
    Low = 10,
    Medium = 15,
    High = 20,
    Highest = 30,
    PowerUp = -20,
}

public enum InputLengthType
{
    None,
    Single,
    Double,
    Triple,
}

public enum HitType
{
    None,
    Face,
    LeftUp,
    LeftDown,
    Middle,
    RightUp,
    RightDown,
}

public enum TagsTypeString
{
    Camera2D,
    Camera3D,
    Player1,
    Player2,
    Pickable,
    Thrown,
    Terrain,
    P1Col,
    P2Col,
}

public enum AudioType
{
    Hit,
    Attack,
    Die,
}