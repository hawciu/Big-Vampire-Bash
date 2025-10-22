using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public GameObject enemyPrefab;

    private int waveNumber = 0;
    private float lastWaveTime = 0f;
    private readonly float waveCooldown = 60;

    private float lastSpawn = 0f;
    private readonly float spawnCooldown = 0.5f;

    private readonly List<GameObject> allEnemies = new();

    public TMP_Text text;

    GameObject currentBoss = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Update()
    {
      
        if (waveNumber >= 5)
        {
            Debug.Log("Wszystkie fale zakoñczone.");
            text.text = "Wszystkie fale zakoñczone.";
            return;
        }

     
        if (Time.time > lastWaveTime + waveCooldown || waveNumber == 0)
        {
            waveNumber++;
            lastWaveTime = Time.time;

            Debug.Log($"=== Fala {waveNumber} ===");

            text.text = $"=== Fala {waveNumber} ===";

            if (waveNumber == 5)
            {
                Debug.Log("To jest fala Bossa!");
                text.text = "To jest fala Bossa!";
                SpawnBoss();
                return; 
            }

            lastSpawn = Time.time;
        }

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
        EnemyDataScriptableObject enemyData = GetRandomEnemyData();

        Vector3 randomLocation = GetRandomSpawnPosition();

        GameObject boss = Instantiate(enemyPrefab, randomLocation, Quaternion.identity);

        EnemySimple simpleEnemy = boss.GetComponent<EnemySimple>();
        if (simpleEnemy != null)
        {
            simpleEnemy.Setup(enemyData, true);
        }
        currentBoss = boss;
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
        List<EnemyDataScriptableObject> enemies = EnemiesDatabaseManager.instance.EnemiesObjects;
        int index = (wave - 1) % enemies.Count; 
        return enemies[index];
    }

    private EnemyDataScriptableObject GetRandomEnemyData()
    {
        List<EnemyDataScriptableObject> enemies = EnemiesDatabaseManager.instance.EnemiesObjects;
        return enemies[Random.Range(0, enemies.Count)];
    }

    internal void RemoveDead(GameObject gameObject)
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
