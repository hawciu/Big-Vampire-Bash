using UnityEngine;

public enum EnemyState
{
    INACTIVE,
    SPAWNING,
    ACTIVE,
    DEAD,
}

public enum EnemyRarity
{
    Base,
    Green,
    Blue,
    Red,
    Gold
}

public class EnemySimple : MonoBehaviour
{
    private EnemyDataScriptableObject enemyData;
    private Rigidbody rb;
    private GameObject modelInstance;
    private bool isBoss = false;
    private int health;
    public GameObject hatPrefab;
    private float whiteFadeCounter = 1;
    private EnemyModelHandler enemyModelHandler;
    private EnemyState state = EnemyState.INACTIVE;
    private CapsuleCollider selfCollider;
    private float overriddenMoveSpeed;

    public EnemyRarity rarity = EnemyRarity.Base;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        EnemyStateHandler();
        if (whiteFadeCounter < 1)
        {
            whiteFadeCounter += Time.deltaTime * 5;
            enemyModelHandler.GetMaterial().color = Color.white * (1 / whiteFadeCounter);
        }
    }

    public void Setup(EnemyDataScriptableObject data)
    {
        enemyData = data;
        health = data.health;
        Debug.Log("[Enemy Spawned] " + enemyData.enemyName + " | HP: " + enemyData.health + " | Speed: " + enemyData.moveSpeed);
        SpawnModel();
        modelInstance.transform.GetChild(0).GetComponent<Animator>().enabled = false;
        enemyModelHandler = modelInstance.GetComponent<EnemyModelHandler>();
        selfCollider = GetComponent<CapsuleCollider>();
        SwitchState(EnemyState.SPAWNING);
    }

    public void SetupEndless(EnemyDataScriptableObject data, int scaledHP, float scaledMoveSpeed)
    {
        enemyData = data;

        health = scaledHP;
        overriddenMoveSpeed = scaledMoveSpeed;

        Debug.Log("[Enemy Spawned - Endless] " + data.enemyName + " | HP: " + health + " | Speed: " + overriddenMoveSpeed);

        SpawnModel();
        modelInstance.transform.GetChild(0).GetComponent<Animator>().enabled = false;

        enemyModelHandler = modelInstance.GetComponent<EnemyModelHandler>();
        selfCollider = GetComponent<CapsuleCollider>();
        whiteFadeCounter = 1f;

        SwitchState(EnemyState.SPAWNING);
    }

    private void SwitchState(EnemyState value)
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
                rb.isKinematic = false;
                selfCollider.enabled = true;
                modelInstance.transform.GetChild(0).GetComponent<Animator>().enabled = true;
                break;

            case EnemyState.DEAD:
                state = EnemyState.DEAD;
                if (isBoss)
                {
                    EnemyManager.instance.OnBossDeath();
                }

                EnemyManager.instance.RemoveDeadEnemy(gameObject);
                modelInstance.transform.GetChild(0).GetComponent<Animator>().enabled = false;
                rb.isKinematic = true;
                selfCollider.enabled = false;
                break;
        }
    }

    private void EnemyStateHandler()
    {
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
                transform.position = transform.position + (Vector3.up * Time.deltaTime * 3);
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
                transform.position = transform.position - (Vector3.up * Time.deltaTime * 3);
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
        GameObject tmp = Instantiate(hatPrefab, modelT.parent);
        tmp.transform.position = modelT.position;
        tmp.transform.rotation = modelT.rotation;
        modelInstance.transform.localScale = Vector3.one * 2f;
        health = 10;
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        if (state != EnemyState.ACTIVE || enemyData == null)
        {
            return;
        }

        rb.velocity = Vector3.zero;


        float speed = overriddenMoveSpeed > 0 ? overriddenMoveSpeed : enemyData.moveSpeed;

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

    private void CheckIfDead()
    {
        if (health <= 0)
        {
            SwitchState(EnemyState.DEAD);
        }
    }

    public void IncreaseRarity()
    {
        if (rarity < EnemyRarity.Gold)
        {
            rarity++;
            UpdateOutlineColor();
        }
    }

    public void UpdateOutlineColor()
    {
        if (enemyModelHandler != null && enemyModelHandler.outlineTarget != null)
        {
            Renderer renderer = enemyModelHandler.outlineTarget.GetComponent<Renderer>();
            if (renderer != null)
            {
                switch (rarity)
                {
                    case EnemyRarity.Base:
                        renderer.material.color = Color.black;
                        break;

                    case EnemyRarity.Green:
                        renderer.material.color = new Color(0f, 0.5f, 0f);
                        break;

                    case EnemyRarity.Blue:
                        renderer.material.color = new Color(0f, 0f, 0.5f);
                        break;

                    case EnemyRarity.Red:
                        renderer.material.color = new Color(0.5f, 0f, 0f);
                        break;

                    case EnemyRarity.Gold:
                        renderer.material.color = new Color(0.7f, 0.6f, 0f);
                        break;
                }
            }
        }
    }
}