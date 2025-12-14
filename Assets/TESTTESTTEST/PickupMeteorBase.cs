using System.Collections;
using UnityEngine;

public class PickupMeteorBase : MonoBehaviour, IPickupEffect
{
    public GameObject meteorPrefab;
    public int meteorCount = 10;
    public float spawnHeight = 6f;
    public float interval = 0.2f;
    public float spawnRadius = 10f;

    public void Activate()
    {
        GameObject player = PlayerManager.instance.GetPlayer();

        if (player != null)
        {
            _ = StartCoroutine(SpawnMeteors(player));
        }

        //  gameObject.SetActive(false);
    }

    private IEnumerator SpawnMeteors(GameObject player)
    {
        for (int i = 0; i < meteorCount; i++)
        {
            Vector2 randCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = player.transform.position +
                               new Vector3(randCircle.x, spawnHeight, randCircle.y);

            _ = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(interval);
        }

        Destroy(gameObject);
    }
}