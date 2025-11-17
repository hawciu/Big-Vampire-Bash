using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [Header("Enemy Prefabs")]
    public GameObject enemyPrefab;

    [Header("Endless Mode Settings")]
    public bool endlessRandomSpawn = false;

    public float endlessSpawnInterval = 10f;

    [Header("Runtime State")]
    private readonly List<GameObject> allEnemies = new();

    private readonly bool gameOver = false;

    private int currentEndlessIndex = 0;
    private float lastTypeChangeTime = 0f;
    public int aliveEnemies = 0;
    private EnemyRarity currentEndlessRarity = EnemyRarity.Base;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void EndlessSpawnUpdate(float scaledHP, float scaledMoveSpeed)
    {
        if (gameOver)
        {
            return;
        }

        int enemiesCount = EnemiesDatabaseManager.instance.EnemiesObjects.Count;
        if (enemiesCount == 0)
        {
            return;
        }

        if (endlessRandomSpawn)
        {
            currentEndlessIndex = Random.Range(0, enemiesCount);
        }
        else if (Time.time > lastTypeChangeTime + endlessSpawnInterval)
        {
            lastTypeChangeTime = Time.time;

            currentEndlessIndex = (currentEndlessIndex + 1) % enemiesCount;

            if (currentEndlessIndex == 0)
            {
                if (currentEndlessRarity < EnemyRarity.Gold)
                {
                    currentEndlessRarity++;
                }
            }
        }

        EnemyDataScriptableObject data = GetEnemyDataForWave(currentEndlessIndex);

        int finalHP = Mathf.FloorToInt(data.health + scaledHP);
        float finalSpeed = data.moveSpeed * scaledMoveSpeed;

        Vector3 pos = GetRandomSpawnPosition();
        GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);

        EnemySimple simpleEnemy = enemy.GetComponent<EnemySimple>();
        simpleEnemy.rarity = currentEndlessRarity;
        simpleEnemy.SetupEndless(data, finalHP, finalSpeed);
        simpleEnemy.UpdateOutlineColor();

        allEnemies.Add(enemy);
        aliveEnemies++;
    }

    public void SpawnEnemy(bool ifBoss)
    {
        if (gameOver)
        {
            return;
        }

        EnemyDataScriptableObject enemyData = GetEnemyDataForWave(LevelManager.instance.GetWaveNumber());
        Vector3 pos = GetRandomSpawnPosition();

        GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
        EnemySimple simple = enemy.GetComponent<EnemySimple>();
        simple.Setup(enemyData);

        if (ifBoss)
        {
            simple.MakeBoss();
        }

        allEnemies.Add(enemy);
    }

    public void OnBossDeath()
    {
        LevelManager.instance.OnMinibossDeath();
    }

    private EnemyDataScriptableObject GetEnemyDataForWave(int wave)
    {
        return EnemiesDatabaseManager.instance.EnemiesObjects[wave];
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float r = LevelManager.instance.GetBounds() - 1f;
        Vector3 p;

        do
        {
            p = new Vector3(Random.Range(-r, r), 0f, Random.Range(-r, r));
        }
        while (Vector3.Distance(PlayerManager.instance.GetPlayer().transform.position, p) < 10f);

        return p;
    }

    public void AddEnemyToAllEnemies(GameObject enemy)
    {
        allEnemies.Add(enemy);
    }

    internal void RemoveDeadEnemy(GameObject enemy)
    {
        _ = allEnemies.Remove(enemy);
        aliveEnemies--;
    }

    public List<GameObject> GetAllEnemies()
    {
        return new List<GameObject>(allEnemies);
    }

    public GameObject GetNearestEnemy(GameObject closestTo)
    {
        if (allEnemies.Count == 0)
        {
            return null;
        }

        GameObject closest = null;
        float best = float.MaxValue;

        foreach (GameObject enemy in allEnemies)
        {
            if (enemy == null)
            {
                continue;
            }

            float d = Vector3.Distance(closestTo.transform.position, enemy.transform.position);
            if (d < best)
            {
                best = d;
                closest = enemy;
            }
        }

        return closest;
    }
}