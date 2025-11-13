using UnityEngine;

public enum EnemyState
{
    INACTIVE,
    SPAWNING,
    ACTIVE,
    DEAD,
}
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
    EnemyState state = EnemyState.INACTIVE;
    CapsuleCollider selfCollider;

    public void Setup(EnemyDataScriptableObject data)
    {
        enemyData = data;
        SpawnModel();
        modelInstance.gameObject.transform.GetChild(0).GetComponent<Animator>().enabled = false;
        enemyModelHandler = modelInstance.GetComponent<EnemyModelHandler>();
        selfCollider = GetComponent<CapsuleCollider>();
        SwitchState(EnemyState.SPAWNING);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        EnemyStateHandler();
        if(whiteFadeCounter < 1)
        {
            whiteFadeCounter += Time.deltaTime * 5;
            enemyModelHandler.GetMaterial().color = Color.white * (1 / whiteFadeCounter);
        }
    }

    void SwitchState(EnemyState value)
    {
        switch (value)
        {
            case EnemyState.INACTIVE:
                state = EnemyState.INACTIVE;
                break;

            case EnemyState.SPAWNING:
                state = EnemyState.SPAWNING;
                transform.position = transform.position - new Vector3(0, 3, 0);
                break;

            case EnemyState.ACTIVE:
                state = EnemyState.ACTIVE;
                EnemyManager.instance.AddEnemyToAllEnemies(gameObject);
                GetComponent<Rigidbody>().isKinematic = false;
                selfCollider.enabled = true;
                modelInstance.gameObject.transform.GetChild(0).GetComponent<Animator>().enabled = true;
                break;

            case EnemyState.DEAD:
                state = EnemyState.DEAD;
                if (isBoss) EnemyManager.instance.OnBossDeath();
                EnemyManager.instance.RemoveDeadEnemy(gameObject);
                modelInstance.gameObject.transform.GetChild(0).GetComponent<Animator>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;
                selfCollider.enabled = false;
                break;
        }
    }

    void EnemyStateHandler()
    {
        switch(state)
        {
            case EnemyState.INACTIVE:
                break;

            case EnemyState.SPAWNING:
                if (transform.position.y >= 0)
                {
                    transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                    SwitchState(EnemyState.ACTIVE);
                    break;
                }
                transform.position = transform.position + Vector3.up * Time.deltaTime * 3;
                break;

            case EnemyState.ACTIVE:
                break;

            case EnemyState.DEAD:
                if (transform.position.y <= -3)
                {
                    SwitchState(EnemyState.INACTIVE);
                    Destroy(gameObject);
                    break;
                }
                transform.position = transform.position - Vector3.up * Time.deltaTime * 3;
                break;
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
        if (state != EnemyState.ACTIVE) return;

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
            SwitchState(EnemyState.DEAD);
        }
    }
}
