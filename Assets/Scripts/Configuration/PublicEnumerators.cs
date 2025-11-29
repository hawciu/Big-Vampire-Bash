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
    GROUND_SPIKE,
}

public enum PortalFunction
{
    ENTER_NOZOOMIN,
    ENTER,
    LEAVE,
}

public enum LevelType
{
    LEVEL1,
    LEVEL2,
}
