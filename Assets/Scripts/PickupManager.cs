using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public static PickupManager instance;

    public GameObject pickupPrefab;
    public PickupDataScriptableObject[] pickupDataList;
    public float spawnCooldown = 10f;

    private float lastSpawnTime = 0f;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        TrySpawnPickup();
    }

    private void TrySpawnPickup()
    {
        if (Time.time < lastSpawnTime + spawnCooldown)
        {
            return;
        }

        float bounds = LevelManager.instance.GetBounds() - 1f;

        Vector3 randomLocation = new(
            Random.Range(-bounds, bounds),
            0f,
            Random.Range(-bounds, bounds)
        );

        PickupDataScriptableObject randomPickupData = GetRandomPickupData();
        if (randomPickupData == null)
        {
            return;
        }

        GameObject pickup = Instantiate(pickupPrefab, randomLocation, Quaternion.identity);

        PickupController controller = pickup.GetComponent<PickupController>();
        if (controller != null)
        {
            controller.Setup(randomPickupData);
        }

        lastSpawnTime = Time.time;
    }

    private PickupDataScriptableObject GetRandomPickupData()
    {
        if (pickupDataList == null || pickupDataList.Length == 0)
        {
            return null;
        }

        int index = Random.Range(0, pickupDataList.Length);
        return pickupDataList[index];
    }
}