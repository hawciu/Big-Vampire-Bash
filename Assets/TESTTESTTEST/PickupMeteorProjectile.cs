using UnityEngine;

public class PickupMeteorProjectile : MonoBehaviour
{
    public float impactRadius = 4f;
    public LayerMask enemyLayer;
    public LayerMask groundLayer;
    private void OnParticleCollision(GameObject other)
    {

        Vector3 explosionPos = new(transform.position.x, 0f, transform.position.z);
        Collider[] hits = Physics.OverlapSphere(explosionPos, impactRadius, enemyLayer);
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