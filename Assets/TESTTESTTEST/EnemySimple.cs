using UnityEngine;

public class EnemySimple : MonoBehaviour
{
    private EnemyDataScriptableObject enemyData;
    private Rigidbody rb;
    private GameObject modelInstance;
    private bool isBoss = false;

    public void Setup(EnemyDataScriptableObject data, bool boss)
    {
        enemyData = data;
        isBoss = boss;
        SpawnModel();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void SpawnModel()
    {
        if (enemyData == null)
        {
            return;
        }

        modelInstance = Instantiate(enemyData.enemyPrefab, transform.position, Quaternion.identity);
        modelInstance.transform.SetParent(transform);
        modelInstance.transform.localPosition = Vector3.zero;

        if (isBoss)
        {
            modelInstance.transform.localScale = Vector3.one * 2f;
        }
    }

    private void FixedUpdate()
    {
        if (enemyData == null)
        {
            return;
        }

        rb.linearVelocity = Vector3.zero;

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
