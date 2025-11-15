using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;

    int coins = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    internal void OnCoinPickup()
    {
        coins++;
        UIManager.instance.UpdateCoinsText(coins.ToString());
    }

    public void PauseGame(bool value)
    {
        Time.timeScale = value ? 0 : 1;
    }

    public void OnGameOver()
    {
        Time.timeScale = 0;
        UIManager.instance.OnGameOver();
    }
}