using System.Collections;
using UnityEngine;

public class PickupDarkExplosionProjectile : MonoBehaviour
{
    public float maxImpactRadius = 10f;
    public float duration = 1f;
    public LayerMask enemyLayer;

    private readonly float baseSize = 20f;

    private void Start()
    {
        _ = StartCoroutine(AnimateExplosion());
    }

    private IEnumerator AnimateExplosion()
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;


            float impactRadius = Mathf.Sin(t * Mathf.PI) * maxImpactRadius;


            float yPos = t;

            Vector3 explosionPos = new(transform.position.x, yPos, transform.position.z);


            if (impactRadius > 0)
            {
                Collider[] hits = Physics.OverlapSphere(explosionPos, impactRadius, enemyLayer);
                foreach (Collider hit in hits)
                {
                    if (hit.TryGetComponent<EnemySimple>(out EnemySimple enemy))
                    {
                        enemy.Damage();
                    }
                }
            }


            transform.localScale = Vector3.one * (baseSize * impactRadius / maxImpactRadius);

            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxImpactRadius);
    }
}
