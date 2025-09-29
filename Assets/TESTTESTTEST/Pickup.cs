using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float destroyRadius = 10f;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, destroyRadius);
            foreach (Collider hit in hitColliders)
            {
                if (hit.CompareTag("Enemy"))
                {
<<<<<<< Updated upstream
                    hit.gameObject.GetComponent<EnemySimple>().Kill();
                    Destroy(hit.gameObject);
=======
                    if (hit.TryGetComponent<EnemySimple>(out EnemySimple enemy))
                    {
                        enemy.Kill();
                    }
>>>>>>> Stashed changes
                }
            }



            Destroy(gameObject);

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destroyRadius);
    }
}