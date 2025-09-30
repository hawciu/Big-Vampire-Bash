using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private float lastShot = 0;
    private readonly float shotCooldown = 2;

    public GameObject projectile;

    private void Update()
    {
        if (lastShot + shotCooldown < Time.time)
        {
            lastShot = Time.time;
            GameObject tmp = EnemyManager.instance.GetNearestEnemy(gameObject);
            GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
            proj.GetComponent<PlayerProjectile>().Setup((tmp.transform.position - transform.position).normalized);
        }
    }
}