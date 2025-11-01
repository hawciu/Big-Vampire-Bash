using System.Collections;
using UnityEngine;

public class RadialBlastEffect : MonoBehaviour, IPickupEffect
{
    public GameObject pickupModel;
    public GameObject bulletPrefab;
    public int waves = 3;
    public int bulletsPerWave = 5;
    public float intervalBetweenWaves = 0.3f;
    public float bulletSpeed = 10f;

    public float spawnHeightOffset = 1.5f;

    public void Activate(GameObject player)
    {
        if (pickupModel != null)
        {
            pickupModel.SetActive(false);
        }

        _ = StartCoroutine(SpawnWaves(player));
    }

    private IEnumerator SpawnWaves(GameObject player)
    {
        for (int w = 0; w < waves; w++)
        {
            Vector3 playerPos = player.transform.position + (Vector3.up * spawnHeightOffset);

            for (int b = 0; b < bulletsPerWave; b++)
            {
                float angle = 360f / bulletsPerWave * b;
                float randomOffset = Random.Range(-5f, 5f);
                angle += randomOffset;

                Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

                GameObject proj = Instantiate(bulletPrefab, playerPos, Quaternion.identity);
                Projectile projectile = proj.GetComponent<Projectile>();
                if (projectile != null)
                {

                    projectile.Setup(direction, 11f);
                }
            }

            yield return new WaitForSeconds(intervalBetweenWaves);
        }

        Destroy(transform.parent.gameObject);
    }

}
