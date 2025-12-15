using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    GameState gameState = GameState.NONE;

    int coins = 0;

    bool gamePaused = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        
        }
    }
    private void Start()
    {
        SwitchState(GameState.SETUP);
    }

    private void Update()
    {
        GameStateUpdate();
    }

    public void SwitchState(GameState targetState)
    {
        gameState = targetState;
        switch (gameState)
        {
            case GameState.SETUP:
                GameSetup();
                break;

            case GameState.WAVE:
                LevelManager.instance.StartWave();
                break;

            case GameState.MINIBOSS:
                EnemyManager.instance.SpawnMiniboss();
                break;

            case GameState.BOSS:
                break;

            case GameState.ENDGAME:
                EffectsManager.instance.SpawnEndGamePortal(EnemyManager.instance.GetLastBossDeathLocation());
                break;
        }
    }

    private void GameStateUpdate()
    {
        switch (gameState)
        {
            case GameState.SETUP:
                break;

            case GameState.WAVE:
                LevelManager.instance.WaveUpdate();
                break;

            case GameState.MINIBOSS:
                LevelManager.instance.MinibossWaveUpdate();
                break;

            case GameState.BOSS:
                //currently skipped
                GameManager.instance.SwitchState(GameState.ENDGAME);
                break;

            case GameState.ENDGAME:
                break;
        }
    }

    void GameSetup()
    {
        LevelManager.instance.SetupLevel();
        coins = SaveManager.instance.GetCoinsAmount();
        PlayerManager.instance.SpawnPlayer();
    }

    internal void OnCoinPickup()
    {
        coins++;
    }

    public void PauseGame(bool pause)
    {
        gamePaused = pause;
        PlayerManager.instance.PausePlayer(pause);
        EnemyManager.instance.PauseAllEnemies(pause);
    }

    public void OnGameOver()
    {
        PauseGame(true);
        SaveManager.instance.SaveCoinsAmount(coins);
        CameraManager.instance.SwitchCameraState(CameraState.ZOOM_PLAYER);
    }

    internal GameState GetGameState()
    {
        return gameState;
    }

    public void OnPortalActionCompleted(PortalFunction portalFunction)
    {
        switch (portalFunction)
        {
            case PortalFunction.LEAVE:
                PlayerManager.instance.EnablePlayerControls(true);
                PlayerManager.instance.EnablePlayerWeapon(true);
                SwitchState(GameState.WAVE);
                break;
            case PortalFunction.ENTER:
                break;
        }
    }

    public bool IsGamePaused() { return gamePaused; }

    internal void OnEndGamePortalEnter()
    {
        PauseGame(true);
        GameObject portalInstance = EffectsManager.instance.SpawnPortal(PlayerManager.instance.GetPlayer().transform.position);
        portalInstance.GetComponent<PortalScript>().SetupPortal(PlayerManager.instance.GetPlayer(), CameraManager.instance.GetPlayerCamera(), PortalFunction.ENTER);

    }

    public void OnContinueButtonPressed()
    {
        PlayerManager.instance.ResurrectPlayer();
        EnemyManager.instance.ClearEnemiesAroundPlayer();
        CameraManager.instance.SwitchCameraState(CameraState.FOLLOW_PLAYER);
        PauseGame(false);
    }
}