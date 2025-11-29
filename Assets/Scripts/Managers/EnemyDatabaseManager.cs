using System.Collections.Generic;
using UnityEngine;

public class EnemyDatabaseManager : MonoBehaviour
{
    public static EnemyDatabaseManager instance;
    public List<EnemyDataScriptableObject> EnemiesObjects = new();
    public List<PlayerDataScriptableObject> PlayersObjects = new();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public PlayerDataScriptableObject GetPlayerByType(PlayerType type)
    {
        foreach (PlayerDataScriptableObject i in PlayersObjects)
        {
            if (i.playerType == type) return i;
        }
        return null;
    }

}