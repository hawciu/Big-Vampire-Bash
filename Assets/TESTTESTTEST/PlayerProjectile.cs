using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public Vector3 moveDirection;
    private Vector3 startLocation;
    private readonly float speed = 11;

    private void Start()
    {
        startLocation = transform.position;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(startLocation, transform.position) > 50)
        {
            Destroy(gameObject);
        }

        GetComponent<Rigidbody>().MovePosition(transform.position + (moveDirection.normalized * Time.deltaTime * speed));
    }

    public void Setup(Vector3 direction)
    {
        moveDirection = direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerManager.instance.GetPlayer())
        {
            return;
        }

        if (other.gameObject.TryGetComponent<EnemySimple>(out EnemySimple t))
        {
            t.Damage();
        }
        Destroy(gameObject);
    }
}