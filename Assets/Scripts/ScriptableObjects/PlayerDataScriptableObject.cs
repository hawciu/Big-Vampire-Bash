using UnityEngine;

public enum PlayerType
{
    BOY,
    ROBOT,
}

[CreateAssetMenu]
public class PlayerDataScriptableObject : ScriptableObject
{
    [Header("Basic Player Info")]
    public string playerName;
    public PlayerType playerType;

    [Tooltip("Player prefab (model + animations)")]
    public GameObject playerModelPrefab;

    [Header("Movement Settings")]
    [Tooltip("Movement speed of the enemy")]
    public float moveSpeed = 1f;
    public int health = 1;
}