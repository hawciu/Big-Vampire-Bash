using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject playerMainPrefab;
    public GameObject playerCamera;

    float playerShotCooldown = 2f;

    GameObject playerInstance;
    PlayerModelHandler playerModelHandler;
    PlayerController playerController;

    bool playerControlsEnabled = false;
    bool playerWeaponEnabled = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void SpawnPlayer()
    {
        playerInstance = Instantiate(playerMainPrefab);
        playerController = playerInstance.GetComponent<PlayerController>();
        GameObject tmp = Instantiate(EnemyDatabaseManager.instance.GetPlayerByType(SaveManager.instance.GetPlayerChoiceType()).playerModelPrefab, playerInstance.transform);
        playerModelHandler = tmp.GetComponent<PlayerModelHandler>();
        playerController.SetPlayerAnimator(playerModelHandler.GetAnimator());

        Instantiate(playerCamera);
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

    internal void DamagePlayer()
    {
        GameManager.instance.OnGameOver();
    }

    public void EnablePlayerControls(bool value)
    {
        playerControlsEnabled = value;
    }

    public void EnablePlayerWeapon(bool value)
    {
        playerWeaponEnabled = value;
    }

    public bool GetPlayerControlsEnabled()
    {
        return playerControlsEnabled;
    }

    internal bool GetPlayerWeaponEnabled()
    {
        return playerWeaponEnabled;
    }
}
