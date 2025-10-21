using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public static PickupManager instance;
    public GameObject pickUp;

    private float lastSpawn = 0;
    private readonly float spawnCooldown = 10;

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
            randomLocation = new Vector3(Random.Range(rnd, -rnd), 0, Random.Range(rnd, -rnd));
        }
        while (Vector3.Distance(PlayerManager.instance.GetPlayer().transform.position, randomLocation) < 10);
        if (Time.time > lastSpawn + spawnCooldown)
        {
            _ = Instantiate(pickUp, randomLocation, Quaternion.identity); ;
            lastSpawn = Time.time;
        }
    }
}