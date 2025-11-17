using UnityEngine;

public enum EnemyType
{
    GOBLIN,
    GUY,
    LADY,
    PIG,
    WRAITH
}

[CreateAssetMenu]
public class EnemyDataScriptableObject : ScriptableObject
{
    [Header("Basic Enemy Info")]
    public string enemyName;

    public EnemyType enemyType;

    [Tooltip("Enemy prefab (model + animations)")]
    public GameObject enemyPrefab;

    [Header("Movement Settings")]
    [Tooltip("Movement speed of the enemy")]
    public float moveSpeed = 1f;

    public int health = 1;
}