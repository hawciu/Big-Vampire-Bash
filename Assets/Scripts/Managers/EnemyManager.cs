using System.Collections.Generic;
using TMPro;
using UnityEngine;


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

    private bool bossAlive = false;
    bool gameOver = false;

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

    private void LevelSetup()
    {
        text.text = $"=== Fala {waveNumber} ===";
        Debug.Log($"=== Fala {waveNumber} ===");
    }

    private void WaveUpdate()
    {
        GameEndCheck();

        NextWaveCheck();

        EnemySpawnCheck();
    }

    private void GameEndCheck()
    {
        if (waveNumber >= 5)
        {
            gameOver = true;
            Debug.Log("Wszystkie fale zakoñczone.");
            text.text = "Wszystkie fale zakoñczone.";
            return;
        }
    }

    private void NextWaveCheck()
    {

        if (bossAlive)
        {
            return;
        }

        if (Time.time > lastWaveTime + waveCooldown)
        {
            Debug.Log("To jest fala Bossa!");
            text.text = "To jest fala Bossa!";
            SpawnEnemy(true);
        }
    }

    public void OnBossDeath()
    {
        bossAlive = false;
        ProgressWave();
    }

    private void ProgressWave()
    {
        waveNumber++;
        lastWaveTime = Time.time;

        Debug.Log($"=== Fala {waveNumber} ===");

        text.text = $"=== Fala {waveNumber} ===";

        lastSpawn = Time.time;
    }

    private void EnemySpawnCheck()
    {
        if (waveNumber != 5 && Time.time > lastSpawn + spawnCooldown)
        {
            SpawnEnemy(false);
            lastSpawn = Time.time;
        }
    }

    private void SpawnEnemy(bool ifBoss)
    {
        if (gameOver) return;
        EnemyDataScriptableObject enemyData = GetEnemyDataForWave(waveNumber);

        Vector3 randomLocation = GetRandomSpawnPosition();

        GameObject enemy = Instantiate(enemyPrefab, randomLocation, Quaternion.identity);

        EnemySimple simpleEnemy = enemy.GetComponent<EnemySimple>();
        if (simpleEnemy != null)
        {
            simpleEnemy.Setup(enemyData);
        }

        if (ifBoss)
        {
            simpleEnemy.MakeBoss();
            bossAlive = true;
        }


        //allEnemies.Add(enemy);
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

    public void AddEnemyToAllEnemies(GameObject gameObject)
    {
        allEnemies.Add(gameObject);
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