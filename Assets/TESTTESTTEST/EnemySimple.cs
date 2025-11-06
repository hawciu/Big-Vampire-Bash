using UnityEngine;

public class EnemySimple : MonoBehaviour
{
    private EnemyDataScriptableObject enemyData;
    private Rigidbody rb;
    private GameObject modelInstance;
    private bool isBoss = false;
    int health = 1;
    public GameObject hatPrefab;

    public void Setup(EnemyDataScriptableObject data)
    {
        enemyData = data;
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
    }

    public void MakeBoss()
    {
        isBoss = true;
        Transform modelT = modelInstance.GetComponent<EnemyModelHandler>().hatTargetObject.transform;
        GameObject  tmp = Instantiate(hatPrefab, modelT.parent);
        tmp.transform.position = modelT.position;
        tmp.transform.rotation = modelT.rotation;
        modelInstance.transform.localScale = Vector3.one * 2f;
        health = 10;
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

    public void Damage()
    {
        health--;
        CheckIfDead();
    }

    void CheckIfDead()
    {
        if(health <= 0)
        {
            EnemyManager.instance.RemoveDeadEnemy(gameObject);
            if (isBoss) EnemyManager.instance.OnBossDeath();
            Destroy(gameObject);
        }
    }
}
