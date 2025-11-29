using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public SaveObjectScriptableObject saveObject;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadOnceAtGameStart();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadOnceAtGameStart()
    {
        if (saveObject.loadedOnce) return;
        saveObject.loadedOnce = true;
        LoadPlayerChoice();
        LoadLevelChoice();
        LoadCoinsAmount();
    }

    private void LoadCoinsAmount()
    {
        saveObject.coinAmount = PlayerPrefs.GetInt("coins");
    }

    public void SaveCoinsAmount(int amount)
    {
        saveObject.coinAmount = amount;
        PlayerPrefs.SetInt("coins", amount);
        PlayerPrefs.Save();
    }

    public void SavePlayerChoice(PlayerType playerType)
    {
        saveObject.PlayerChoice = playerType;
        PlayerPrefs.SetInt("playerChoice", (int)playerType);
    }

    public void LoadPlayerChoice()
    {
        saveObject.levelChoice = (LevelType)PlayerPrefs.GetInt("levelChoice");
    }

    public void SaveLevelChoice(LevelType levelType)
    {
        saveObject.levelChoice = levelType;
        PlayerPrefs.SetInt("levelChoice", (int)levelType);
    }

    public void LoadLevelChoice()
    {
        saveObject.levelChoice = (LevelType)PlayerPrefs.GetInt("levelChoice");
    }

    public void Activate()
    {
        print("save manager activation");
    }

    internal int GetCoinsAmount()
    {
        return saveObject.coinAmount;
    }

    public PlayerType GetPlayerChoiceType()
    {
        return saveObject.PlayerChoice;
    }
}
