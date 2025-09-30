using UnityEngine;

public class EnemySimple : MonoBehaviour
{
    private EnemyData enemyData;
    private Rigidbody rb;
    private GameObject modelInstance;

    private void Awake()
    {
        SpawnRandomEnemy();

        rb = GetComponent<Rigidbody>();
    }

    private void SpawnRandomEnemy()
    {
        enemyData = EnemiesDatabaseManager.instance.EnemiesObjects[
            Random.Range(0, EnemiesDatabaseManager.instance.EnemiesObjects.Count)
        ];

        modelInstance = Instantiate(enemyData.enemyPrefab, transform.position, Quaternion.identity);
        modelInstance.transform.SetParent(transform);
        modelInstance.transform.localPosition = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (enemyData == null)
        {
            return;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

        float speed = enemyData.moveSpeed;
        Vector3 moveDirection = PlayerManager.instance.GetPlayer().transform.position - transform.position;
        moveDirection.y = 0;

        rb.MovePosition(transform.position + (moveDirection.normalized * speed * Time.fixedDeltaTime));
        rb.MoveRotation(Quaternion.LookRotation(moveDirection.normalized));
    }

    public void Kill()
    {
        EnemyManager.instance.RemoveDead(gameObject);
        Destroy(gameObject);
    }
}