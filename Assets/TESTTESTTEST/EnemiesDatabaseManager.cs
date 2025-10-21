using System.Collections.Generic;
using UnityEngine;

public class EnemiesDatabaseManager : MonoBehaviour
{
    public static EnemiesDatabaseManager instance;
    public List<EnemyDataScriptableObject> EnemiesObjects = new();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}