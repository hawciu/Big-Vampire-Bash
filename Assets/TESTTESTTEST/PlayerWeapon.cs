using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    float lastShot = 0;
    float shotCooldown = 2;

    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lastShot + shotCooldown < Time.time)
        {
            lastShot = Time.time;
            GameObject tmp = EnemyManager.instance.GetNearestEnemy(gameObject);
            GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
            proj.GetComponent<PlayerProjectile>().Setup((tmp.transform.position - transform.position).normalized);
        }    
    }
}
