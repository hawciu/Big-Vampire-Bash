using UnityEngine;

public class PickupMeteorProjectile : MonoBehaviour
{
    public float impactRadius = 4f;
    public LayerMask enemyLayer;

    private void OnParticleCollision(GameObject other)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, impactRadius, enemyLayer);
        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<EnemySimple>(out EnemySimple enemy))
            {
                enemy.Damage();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
}