using System.Collections.Generic;
using UnityEngine;

public class EnemyDatabaseManager : MonoBehaviour
{
    public static EnemyDatabaseManager instance;
    public List<EnemyDataScriptableObject> EnemiesObjects = new();
    //public List<PlayerDataScriptableObject> PlayersObjects = new();

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