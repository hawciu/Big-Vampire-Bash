using UnityEngine;

public class PickupRadialblastProjectile : MonoBehaviour
{
    public float speed = 11f;
    public float damage = 20f;

    private Vector3 moveDirection;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = true;
        rb.useGravity = false;

        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            col = gameObject.AddComponent<SphereCollider>();
        }

        col.isTrigger = true;
    }

    public void Setup(Vector3 direction, float speed)
    {
        moveDirection = direction.normalized;
        this.speed = speed;

    }

    private void FixedUpdate()
    {

        rb.MovePosition(transform.position + (moveDirection * speed * Time.fixedDeltaTime));


        if (Vector3.Distance(transform.position, Vector3.zero) > 50f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            return;
        }

        EnemySimple enemy = other.GetComponentInParent<EnemySimple>();
        if (enemy != null)
        {
            enemy.Damage();
            Destroy(gameObject);
        }
    }
}
