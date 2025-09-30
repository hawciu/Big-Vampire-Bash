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
public class EnemyData : ScriptableObject
{
    [Header("Basic Enemy Info")]
    public EnemyType enemyType;

    [Tooltip("Enemy prefab (model + animations)")]
    public GameObject enemyPrefab;

    [Header("Movement Settings")]
    [Tooltip("Movement speed of the enemy")]
    public float moveSpeed = 3f;
}