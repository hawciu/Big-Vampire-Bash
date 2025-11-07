using UnityEngine;

public class EnemySimple : MonoBehaviour
{
    private EnemyDataScriptableObject enemyData;
    private Rigidbody rb;
    private GameObject modelInstance;
    private bool isBoss = false;
    int health = 3;
    public GameObject hatPrefab;
    float whiteFadeCounter = 1;
    EnemyModelHandler enemyModelHandler;
    bool isDead = true;

    public void Setup(EnemyDataScriptableObject data)
    {
        enemyData = data;
        SpawnModel();
        enemyModelHandler = modelInstance.GetComponent<EnemyModelHandler>();
        isDead = false;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(whiteFadeCounter < 1)
        {
            whiteFadeCounter += Time.deltaTime * 5;
            enemyModelHandler.GetMaterial().color = Color.white * (1 / whiteFadeCounter);
        }
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
        if (isDead) return;

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
        whiteFadeCounter = 0;
        CheckIfDead();
    }

    void CheckIfDead()
    {
        if(health <= 0)
        {
            EnemyManager.instance.RemoveDeadEnemy(gameObject);
            if (isBoss) EnemyManager.instance.OnBossDeath();
            isDead = false;
        }
    }
}
