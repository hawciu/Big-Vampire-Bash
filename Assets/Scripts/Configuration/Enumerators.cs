using UnityEngine;

public enum GameState
{
    NONE,
    SETUP,
    WAVE,
    MINIBOSS,
    BOSS,
    ENDGAME,
}

public enum EnemyState
{
    INACTIVE,
    SPAWNING,
    ACTIVE,
    DEAD,
}

public enum PlayerType
{
    BOY,
    ROBOT,
}

public enum EnemyType
{
    GOBLIN,
    GUY,
    LADY,
    PIG,
    WRAITH
}

public enum EnemyMaterialType
{
    ORANGE,
    BLUE,
    GRAY,
    OUTLINE_BLACK,
    OUTLINE_GOLD,
}

public enum ParticleType
{
    ARROW_EXPLOSION,
    PICKUP_PICKUP,
    COIN_PICKUP,
}

public enum PortalState
{
    NONE,
    MOVE_CAMERA_TO_PORTAL,
    CIRCLING,
    OPENING,
    TELEPORTING,
    CLOSING,
    MOVE_CAMERA_BACK,
}

public enum PortalFunction
{
    ENTER_NOZOOMIN,
    ENTER,
    LEAVE,
}
