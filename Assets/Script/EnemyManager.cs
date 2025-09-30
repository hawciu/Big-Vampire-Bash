using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public GameObject enemyPrefab;


    private float lastSpawn = 0;
    private readonly float spawnCooldown = 1f;

    private readonly List<GameObject> allEnemies = new();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        float rnd = LevelManager.instance.GetBounds() - 1;
        Vector3 randomLocation;
        do
        {
            randomLocation = new Vector3(Random.Range(-rnd, rnd), 0, Random.Range(-rnd, rnd));
        }
        while (Vector3.Distance(PlayerManager.instance.GetPlayer().transform.position, randomLocation) < 10);

        if (Time.time > lastSpawn + spawnCooldown)
        {
            GameObject enemy = Instantiate(enemyPrefab, randomLocation, Quaternion.identity);
            allEnemies.Add(enemy);
            lastSpawn = Time.time;
        }
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
        float currentDistance;
        foreach (GameObject i in allEnemies)
        {
            currentDistance = Vector3.Distance(closestTo.transform.position, i.transform.position);
            if (currentDistance < closestDistance)
            {
                closest = i;
            }
        }
        return closest;
    }
}