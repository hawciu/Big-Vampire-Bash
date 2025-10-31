using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public GameObject enemyPrefab;

    private int waveNumber = 0;
    private float lastWaveTime = 0;
    private readonly float waveCooldown = 5;

    private float lastSpawn = 0f;
    private readonly float spawnCooldown = 0.5f;

    private readonly List<GameObject> allEnemies = new();

    public TMP_Text text;

    bool bossAlive = false;

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
        LevelSetup();
    }

    private void Update()
    {
        WaveUpdate();
    }

    void LevelSetup()
    {
        text.text = $"=== Fala {waveNumber} ===";
        Debug.Log($"=== Fala {waveNumber} ===");
    }

    void WaveUpdate()
    {
        GameEndCheck();

        NextWaveCheck();

        EnemySpawnCheck();
    }

    void GameEndCheck()
    {
        if (waveNumber >= 5)
        {
            Debug.Log("Wszystkie fale zakoñczone.");
            text.text = "Wszystkie fale zakoñczone.";
            return;
        }
    }

    void NextWaveCheck()
    {
        if (bossAlive) return;

        if (Time.time > lastWaveTime + waveCooldown)
        {
            Debug.Log("To jest fala Bossa!");
            text.text = "To jest fala Bossa!";
            SpawnBoss();
        }
    }

    public void OnBossDeath()
    {
        bossAlive = false;
        ProgressWave();
    }

    void ProgressWave()
    {
        waveNumber++;
        lastWaveTime = Time.time;

        Debug.Log($"=== Fala {waveNumber} ===");

        text.text = $"=== Fala {waveNumber} ===";

        lastSpawn = Time.time;
    }

    void EnemySpawnCheck()
    {
        if (waveNumber != 5 && Time.time > lastSpawn + spawnCooldown)
        {
            SpawnEnemy();
            lastSpawn = Time.time;
        }
    }

    private void SpawnEnemy()
    {
        EnemyDataScriptableObject enemyData = GetEnemyDataForWave(waveNumber);

        Vector3 randomLocation = GetRandomSpawnPosition();

        GameObject enemy = Instantiate(enemyPrefab, randomLocation, Quaternion.identity);

        EnemySimple simpleEnemy = enemy.GetComponent<EnemySimple>();
        if (simpleEnemy != null)
        {
            simpleEnemy.Setup(enemyData, false);
        }

        allEnemies.Add(enemy);
    }

    private void SpawnBoss()
    {
        bossAlive = true;

        EnemyDataScriptableObject enemyData = GetEnemyDataForWave(waveNumber);

        Vector3 randomLocation = GetRandomSpawnPosition();

        GameObject boss = Instantiate(enemyPrefab, randomLocation, Quaternion.identity);

        EnemySimple simpleEnemy = boss.GetComponent<EnemySimple>();
        if (simpleEnemy != null)
        {
            simpleEnemy.Setup(enemyData, true);
        }
        allEnemies.Add(boss);
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
        return EnemiesDatabaseManager.instance.EnemiesObjects[wave];
    }

    internal void RemoveDeadEnemy(GameObject gameObject)
    {
        _ = allEnemies.Remove(gameObject);
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
}
