using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    AoEDamage,
    Lightning,
    Meteor,
    FireRate
}

[CreateAssetMenu(fileName = "NewPickup", menuName = "Pickups/Pickup Data")]
public class PickupDataScriptableObject : ScriptableObject
{
    [Header("Basic Info")]
    public string pickupName = "New Pickup";

    public PickupType type;
    [TextArea] public string description;

    [Header("Models")]
    public GameObject pickupModelPrefab;

    public GameObject effectModelPrefab;

    [Header("Effect Parameters")]
    public float radius = 10f;

    public int damage = 50;
    public int meteorCount = 10;
    public float duration = 5f;
    public float interval = 0.5f;
    public float fireRateBoost = 0.5f;

    public void Activate(GameObject player)
    {
        switch (type)
        {
            case PickupType.AoEDamage:
                DoAoEDamage(player);
                break;

            case PickupType.Lightning:
                DoLightning(player);
                break;

            case PickupType.Meteor:
                DoMeteor(player);
                break;

            case PickupType.FireRate:
                DoFireRateBoost(player);
                break;
        }
    }

    private void DoAoEDamage(GameObject player)
    {
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, radius);
        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                if (hit.TryGetComponent<EnemySimple>(out EnemySimple enemy))
                {
                    enemy.Damage();
                }
            }
        }
    }

    private void DoLightning(GameObject player)
    {
        Debug.Log("Lightning effect not implemented yet.");
    }

    private void DoMeteor(GameObject player)
    {
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

        Vector3 playerPos = player.transform.position;
        enemies.Sort((a, b) =>
            Vector3.Distance(playerPos, a.transform.position)
            .CompareTo(Vector3.Distance(playerPos, b.transform.position))
        );

        int count = Mathf.Min(meteorCount, enemies.Count);

        for (int i = 0; i < count; i++)
        {
            GameObject enemy = enemies[i];
            if (enemy == null)
            {
                continue;
            }

            Vector3 spawnPos = enemy.transform.position + (Vector3.up * 25f);

            if (effectModelPrefab != null)
            {
                GameObject meteor = Object.Instantiate(effectModelPrefab, spawnPos, Quaternion.identity);

                if (meteor.TryGetComponent<MeteorProjectile>(out MeteorProjectile projectile))
                {
                    projectile.Setup(enemy.transform);
                }
            }

            yield return new WaitForSeconds(interval);
        }
    }

    private void DoFireRateBoost(GameObject player)
    {
        Debug.Log("Fire rate boost not implemented yet.");
    }
}