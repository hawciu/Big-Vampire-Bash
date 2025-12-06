using System;
using UnityEditor;
using UnityEngine;

public enum EnemyState
{
    INACTIVE,
    SPAWNING,
    ACTIVE,
    DEAD,
    KNOCKBACK,
}

public class EnemySimple : MonoBehaviour, IPausable
{
    private EnemyDataScriptableObject enemyData;
    private Rigidbody rb;
    private GameObject modelInstance;
    private bool isBoss = false;
    int health = 3;
    public GameObject hatPrefab;
    float whiteFadeCounter = 1;
    EnemyModelHandler enemyModelHandler;
    public EnemyState state = EnemyState.INACTIVE;
    CapsuleCollider selfCollider;

    float rotationCounter = 0;
    float knockbackTimer = 0;
    bool isGolden = false;

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
    }

    void SwitchState(EnemyState value)
    {
        state = value;
        switch (state)
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

            case EnemyState.KNOCKBACK:
                break;

            case EnemyState.DEAD:
                state = EnemyState.DEAD;
                SetEnemyMaterial(EnemyManager.instance.GetMaterial());
                if (isBoss) EnemyManager.instance.OnBossDeath(transform.position);
                EnemyManager.instance.RemoveDeadEnemy(gameObject);
                modelInstance.gameObject.transform.GetChild(0).GetComponent<Animator>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;
                selfCollider.enabled = false;
                if (isGolden) PickupManager.instance.SpawnGoldCoin(transform.position);
                break;
        }
    }

    void EnemyStateHandler()
    {
        if (GameManager.instance.IsGamePaused()) return;
        if (whiteFadeCounter < 1)
        {
            whiteFadeCounter += Time.deltaTime * 5;
            enemyModelHandler.GetMaterial().color = Color.white * (1 / whiteFadeCounter);
        }
        switch (state)
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

            case EnemyState.KNOCKBACK:
                if (state == EnemyState.KNOCKBACK)
                {
                    knockbackTimer -= Time.deltaTime;
                    if (knockbackTimer < 0)
                    {
                        rb.linearVelocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        SwitchState(EnemyState.ACTIVE);
                    }
                    return;
                }
                break;

            case EnemyState.DEAD:
                if (rotationCounter < 1)
                {
                    rotationCounter += Time.deltaTime * 2;
                    modelInstance.transform.Rotate(Vector3.left * Time.deltaTime * 100 * 2);
                }
                else {
                    if (transform.position.y <= -3)
                    {
                        SwitchState(EnemyState.INACTIVE);
                        Destroy(gameObject);
                        break;
                    }
                    transform.position = transform.position - Vector3.up * Time.deltaTime * 3;
                }
                break;
        }
    }

    private void SpawnModel()
    {
        if (enemyData == null)
        {
            return;
        }

        modelInstance = Instantiate(enemyData.enemyModelPrefab, transform.position, Quaternion.identity);
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
        if (GameManager.instance.IsGamePaused()) return;
        switch (state)
        {
            case EnemyState.ACTIVE:
                rb.linearVelocity = Vector3.zero;

                float speed = enemyData.moveSpeed;
                Vector3 moveDirection = PlayerManager.instance.GetPlayer().transform.position - transform.position;
                moveDirection.y = 0;

                rb.MovePosition(transform.position + (moveDirection.normalized * speed * Time.fixedDeltaTime));
                rb.MoveRotation(Quaternion.LookRotation(moveDirection.normalized));
                break;
        }        
    }

    public void Damage(int amount = 1)
    {
        health -= amount;
        whiteFadeCounter = 0;
        CheckIfDead();
    }

    void CheckIfDead()
    {
        if (health <= 0)
        {
            SwitchState(EnemyState.DEAD);
        }
    }


    public void SetEnemyMaterial(Material material)
    {
        enemyModelHandler.SetMaterial(material);
    }

    internal void MakeGolden()
    {
        isGolden = true;
        enemyModelHandler.SetMaterialOutline(MaterialManager.instance.GetMaterial(EnemyMaterialType.OUTLINE_GOLD));
    }

    public void Pause(bool pause)
    {
        rb.isKinematic = pause;
        enemyModelHandler.PauseAnimator(pause);
    }

    public void KnockBack(float time, Vector3 direction)
    {
        SwitchState(EnemyState.KNOCKBACK);
        knockbackTimer = time;
        rb.AddForce(direction.normalized * 5, ForceMode.Impulse);
    }
}
