using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    public GameObject enemy;

    float lastSpawn = 0;
    float spawnCooldown = 1;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
            Instantiate(enemy, randomLocation, Quaternion.identity); ;
            lastSpawn = Time.time;
        }
    }
}
