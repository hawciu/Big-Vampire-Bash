using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickupData
{
    public string pickupName;
    public GameObject pickupCollectablePrefab;
    public float spawnCooldown = 5f;

    [HideInInspector]
    public float lastSpawnTime;
}

public class PickupManager : MonoBehaviour
{
    public static PickupManager instance;

    public GameObject pickupPrefab;

    public List<PickupData> pickups = new();

    public GameObject pickupCoinPrefab;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        PickupSpawnUpdate();
    }

    private void PickupSpawnUpdate()
    {
        if (pickups.Count == 0)
        {
            return;
        }

        for (int i = 0; i < pickups.Count; i++)
        {
            PickupData data = pickups[i];

            if (Time.time < data.lastSpawnTime + data.spawnCooldown)
            {
                continue;
            }

            float bounds = LevelManager.instance.GetBounds() - 1f;
            Vector3 spawnPos = new(
                Random.Range(-bounds, bounds),
                0f,
                Random.Range(-bounds, bounds)
            );

            GameObject pickup = Instantiate(pickupPrefab, spawnPos, Quaternion.identity);
            GameObject effectInstance = Instantiate(data.pickupCollectablePrefab, pickup.transform);
            effectInstance.transform.localPosition = Vector3.zero;

            PickupBaseController controller = pickup.GetComponent<PickupBaseController>();
            if (controller != null)
            {
                effectInstance.SetActive(false);
                controller.effectChild = effectInstance;
            }

            data.lastSpawnTime = Time.time;
        }
    }

    internal void SpawnGoldCoin(Vector3 position)
    {
        if (pickupCoinPrefab != null)
        {
            GameObject pickup = Instantiate(pickupPrefab, position, Quaternion.identity);
            GameObject effectInstance = Instantiate(pickupCoinPrefab, pickup.transform);
            effectInstance.transform.localPosition = Vector3.zero;

            PickupBaseController controller = pickup.GetComponent<PickupBaseController>();
            if (controller != null)
            {
                controller.effectChild = effectInstance;
            }
        }
    }
}