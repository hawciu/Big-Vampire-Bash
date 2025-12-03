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
                IngameUIManager.instance.UpdateWaveText("Przygotowanie poziomu");
                GameSetup();
                break;

            case GameState.WAVE:
                LevelManager.instance.StartWaves();
                IngameUIManager.instance.UpdateWaveText($"=== Fala {LevelManager.instance.GetWaveNumber()} ===");
                break;

            case GameState.MINIBOSS:
                IngameUIManager.instance.UpdateWaveText("To jest fala Minibossa!");
                EnemyManager.instance.SpawnMiniboss();
                break;

            case GameState.BOSS:
                IngameUIManager.instance.UpdateWaveText("To jest fala Bossa!");
                break;

            case GameState.ENDGAME:
                IngameUIManager.instance.UpdateWaveText("Wszystkie fale zakoñczone.");
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
        IngameUIManager.instance.UpdateCoinsText(coins);
        PlayerManager.instance.SpawnPlayer();


    }

    internal void OnCoinPickup()
    {
        coins++;
        IngameUIManager.instance.UpdateCoinsText(coins);
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
        IngameUIManager.instance.OnGameOver();
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
                IngameUIManager.instance.EnableCanvas(true);
                PlayerManager.instance.EnablePlayerControls(true);
                PlayerManager.instance.EnablePlayerWeapon(true);
                SwitchState(GameState.WAVE);
                break;
            case PortalFunction.ENTER:
                break;
        }
    }

    public bool IsGamePaused() { return gamePaused; }
}