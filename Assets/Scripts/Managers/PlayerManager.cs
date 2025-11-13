using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject playerPrefab;
    public GameObject playerCamera;

    float playerShotCooldown = 0.2f;


    GameObject playerInstance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInstance = Instantiate(playerPrefab);
        Instantiate(playerCamera);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetPlayer()
    {
        return playerInstance;
    }

    public void UpgradePlayerWeapon()
    {
        playerShotCooldown -= 0.5f;

        if (playerShotCooldown <= 0.5f) playerShotCooldown = 0.5f;
    }

    internal float GetShotCooldown()
    {
        return playerShotCooldown;
    }
}
