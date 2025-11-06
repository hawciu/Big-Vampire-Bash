using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private float lastShot = 0;

    public GameObject projectile;

    private void Update()
    {
        if (lastShot + PlayerManager.instance.GetShotCooldown() < Time.time)
        {
            lastShot = Time.time;

            GameObject tmp = EnemyManager.instance.GetNearestEnemy(gameObject);

            Vector3 direction;

            if (tmp != null)
            {
                direction = (tmp.transform.position - transform.position).normalized;
            }
            else
            {
                float randomAngle = Random.Range(0f, 360f);
                direction = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), 0f, Mathf.Sin(randomAngle * Mathf.Deg2Rad)).normalized;
            }

            GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
            proj.GetComponent<PlayerProjectile>().Setup(direction);
        }
    }
}