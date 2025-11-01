using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorEffect : MonoBehaviour, IPickupEffect
{
    public GameObject pickupModel;
    public GameObject meteorPrefab;
    public int meteorCount = 10;
    public float spawnHeight = 25f;
    public float interval = 0.2f;

    public void Activate(GameObject player)
    {
        if (pickupModel != null)
        {
            pickupModel.SetActive(false);
        }

        MonoBehaviour mono = player.GetComponent<MonoBehaviour>();
        if (mono != null)
        {
            _ = mono.StartCoroutine(SpawnMeteors(player));
        }
    }

    private IEnumerator SpawnMeteors(GameObject player)
    {
        List<GameObject> enemies = EnemyManager.instance.GetAllEnemies();
        if (enemies == null || enemies.Count == 0)
        {
            yield break;
        }

        int count = Mathf.Min(meteorCount, enemies.Count);

        for (int i = 0; i < count; i++)
        {
            GameObject enemy = enemies[i];
            if (enemy == null)
            {
                continue;
            }

            Vector3 spawnPos = enemy.transform.position + (Vector3.up * spawnHeight);
            GameObject meteor = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);

            MeteorProjectile projectile = meteor.GetComponent<MeteorProjectile>();
            if (projectile != null)
            {
                projectile.Setup(enemy.transform);
            }

            yield return new WaitForSeconds(interval);
        }

        Destroy(transform.parent.gameObject);
    }
}