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
}