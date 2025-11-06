using UnityEngine;

public class PickupMeteorProjectile : MonoBehaviour
{
    private Transform target;
    private readonly float speed = 15f;
    private readonly float damageRadius = 2f;

    public void Setup(Transform newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < damageRadius)
        {
            if (target.TryGetComponent<EnemySimple>(out EnemySimple enemy))
            {
                enemy.Damage();
            }
            Destroy(gameObject);
        }
    }
}