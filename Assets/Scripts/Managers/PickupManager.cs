using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public static PickupManager instance;

    public GameObject pickupPrefab;
    public List<GameObject> effectPrefabs = new();
    public GameObject pickupCoinPrefab;
    public float spawnCooldown = 5f;

    private float lastSpawnTime = 0f;


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
        if (GameManager.instance.IsGamePaused()) lastSpawnTime += Time.deltaTime;

        if (effectPrefabs.Count > 0)
        {
            if (Time.time < lastSpawnTime + spawnCooldown)
            {
                return;
            }

            float bounds = LevelManager.instance.GetBounds() - 1f;
            Vector3 spawnPos = new(
                Random.Range(-bounds, bounds),
                0f,
                Random.Range(-bounds, bounds)
            );

            GameObject pickup = Instantiate(pickupPrefab, spawnPos, Quaternion.identity);
            int index = Random.Range(0, effectPrefabs.Count);
            GameObject effectInstance = Instantiate(effectPrefabs[index], pickup.transform);
            effectInstance.transform.localPosition = Vector3.zero;

            PickupBaseController controller = pickup.GetComponent<PickupBaseController>();
            if (controller != null)
            {
                controller.effectChild = effectInstance;
            }
        }

        lastSpawnTime = Time.time;
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