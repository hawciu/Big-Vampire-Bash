using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject playerMainPrefab;
    public GameObject playerCamera;

    float playerShotCooldown = 0f;

    GameObject playerInstance;
    GameObject portalInstance;
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

        CameraManager.instance.SpawnPlayerCamera();
        CameraManager.instance.ScreenRotationCheck();
        CameraManager.instance.UpdatePlayerFollowCamera();

        portalInstance = EffectsManager.instance.SpawnPortal(playerInstance.transform.position);
        portalInstance.GetComponent<PortalScript>().SetupPortal(playerInstance, CameraManager.instance.GetPlayerCamera(), PortalFunction.LEAVE);
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

    internal void PausePlayer(bool pause)
    {
        playerWeaponEnabled = !pause;
        EnablePlayerControls(!pause);
        playerController.Pause(pause);
    }

    public GameObject GetPlayerCameraZoomTarget()
    {
        return playerController.GetPlayerCameraZoomTarget();
    }
    public GameObject GetPlayerCameraZoomTargetPivot()
    {
        return playerController.GetPlayerCameraZoomTargetPivot();
    }
}
