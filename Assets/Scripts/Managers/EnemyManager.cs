using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public GameObject enemyPrefab;

    public List<GameObject> allEnemies = new();

    bool gameOver = false;

    public Material orange, blue, gray;
    int enemyNameIndex = 0;
    private Vector3 lastBossLocation;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void OnBossDeath(Vector3 position)
    {
        lastBossLocation = position;
        LevelManager.instance.OnMinibossDeath();
    }

    public void SpawnEnemy(bool ifBoss)
    {
        if (gameOver) return;
        EnemyDataScriptableObject enemyData = GetEnemyDataForWave(LevelManager.instance.GetWaveNumber());

        Vector3 randomLocation = GetRandomSpawnPosition();

        GameObject enemy = Instantiate(enemyPrefab, randomLocation, Quaternion.identity);
        enemy.name = enemy.name+enemyData.name + enemyNameIndex;
        enemyNameIndex++;

        EnemySimple simpleEnemy = enemy.GetComponent<EnemySimple>();

        if (simpleEnemy != null)
        {
            simpleEnemy.Setup(enemyData);
        }

        if (ifBoss)
        {
            simpleEnemy.MakeBoss();
        }
        else
        {
            if(Random.Range(0,10) == 5)
            {
                simpleEnemy.MakeGolden();
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float rnd = LevelManager.instance.GetBounds() - 1f;
        Vector3 randomLocation;
        do
        {
            randomLocation = new Vector3(Random.Range(-rnd, rnd), 0f, Random.Range(-rnd, rnd));
        }
        while (Vector3.Distance(PlayerManager.instance.GetPlayer().transform.position, randomLocation) < 10f);

        return randomLocation;
    }

    private EnemyDataScriptableObject GetEnemyDataForWave(int wave)
    {
        return EnemyDatabaseManager.instance.EnemiesObjects[wave];
    }

    internal void RemoveDeadEnemy(GameObject gameObject)
    {
        allEnemies.Remove(gameObject);
    }

    public void AddEnemyToAllEnemies(GameObject gameObject)
    {
        if (allEnemies.Contains(gameObject)) return;
        allEnemies.Add(gameObject);
    }

    public List<GameObject> GetAllEnemies()
    {
        return allEnemies;
    }

    public GameObject GetNearestEnemy(GameObject closestTo)
    {
        if (allEnemies.Count == 0)
        {
            return null;
        }

        GameObject closest = allEnemies[0];
        float closestDistance = Vector3.Distance(closestTo.transform.position, closest.transform.position);

        foreach (GameObject i in allEnemies)
        {
            float currentDistance = Vector3.Distance(closestTo.transform.position, i.transform.position);
            if (currentDistance < closestDistance)
            {
                closest = i;
                closestDistance = currentDistance;
            }
        }
        return closest;
    }

    public Material GetMaterial()
    {
        return gray;
    }

    internal void SpawnMiniboss()
    {
        SpawnEnemy(true);
    }

    internal void PauseAllEnemies(bool pause)
    {
        foreach (GameObject i in allEnemies)
        {
            i.GetComponent<EnemySimple>().Pause(pause);
        }
    }

    public Vector3 GetLastBossDeathLocation()
    {
        return lastBossLocation;
    }

    internal void ClearEnemiesAroundPlayer()
    {
        List<GameObject> enemiesToClear = new();
        foreach(GameObject i in allEnemies)
        {
            if(Vector3.Distance(i.transform.position, PlayerManager.instance.GetPlayer().transform.position) < 10)
            {
                enemiesToClear.Add(i);
            }
        }
        Vector3 direction;
        PauseAllEnemies(false);
        foreach(GameObject i in enemiesToClear)
        {
            direction = (i.transform.position - PlayerManager.instance.GetPlayer().transform.position).normalized;
            i.GetComponent<EnemySimple>().KnockBack(1, direction);
        }
    }
}