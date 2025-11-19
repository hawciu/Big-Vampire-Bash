using System;
using UnityEngine;

public enum GameState
{
    NONE,
    SETUP,
    WAVE,
    MINIBOSS,
    BOSS,
    ENDGAME,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    GameState gameState = GameState.NONE;

    public GameObject player;

    int coins = 0;

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

    public void SwitchState(GameState targetState)
    {
        gameState = targetState;
        switch (gameState)
        {
            case GameState.SETUP:
                UIManager.instance.UpdateWaveText("Przygotowanie poziomu");
                coins = SaveManager.instance.LoadCoinsAmount();
                UIManager.instance.UpdateCoinsText(coins);
                break;

            case GameState.WAVE:
                UIManager.instance.UpdateWaveText($"=== Fala {LevelManager.instance.GetWaveNumber()} ===");
                break;

            case GameState.MINIBOSS:
                UIManager.instance.UpdateWaveText("To jest fala Minibossa!");
                EnemyManager.instance.SpawnMiniboss();
                break;

            case GameState.BOSS:
                UIManager.instance.UpdateWaveText("To jest fala Bossa!");
                break;

            case GameState.ENDGAME:
                UIManager.instance.UpdateWaveText("Wszystkie fale zakoñczone.");
                break;
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    internal void OnCoinPickup()
    {
        coins++;
        UIManager.instance.UpdateCoinsText(coins);
    }

    public void PauseGame(bool value)
    {
        Time.timeScale = value ? 0 : 1;
    }

    public void OnGameOver()
    {
        Time.timeScale = 0;
        SaveManager.instance.SaveCoinsAmount(coins);
        UIManager.instance.OnGameOver();
    }

    internal GameState GetGameState()
    {
        return gameState;
    }
}