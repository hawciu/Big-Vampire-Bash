using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject playerPrefab;
    public GameObject playerCamera;

    private float playerShotCooldown = 0.02f;

    private GameObject playerInstance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        playerInstance = Instantiate(playerPrefab);
        _ = Instantiate(playerCamera);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public GameObject GetPlayer()
    {
        return playerInstance;
    }

    public void UpgradePlayerWeapon()
    {
        playerShotCooldown -= 0.5f;

        if (playerShotCooldown <= 0.5f)
        {
            playerShotCooldown = 0.5f;
        }
    }

    internal float GetShotCooldown()
    {
        return playerShotCooldown;
    }

    internal void DamagePlayer()
    {
        GameManager.instance.OnGameOver();
    }
}