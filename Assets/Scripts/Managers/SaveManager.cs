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
        //load saves at game start
    }

    public int LoadCoinsAmount()
    {
        return PlayerPrefs.GetInt("coins");
    }

    public void SaveCoinsAmount(int amount)
    {
        PlayerPrefs.SetInt("coins", amount);
        PlayerPrefs.Save();
    }

    public void SavePlayerChoice(PlayerType playerType)
    {
        PlayerPrefs.SetInt("playerChoice", (int)playerType);
    }

    public PlayerType LoadPlayerChoice()
    {
        return (PlayerType)PlayerPrefs.GetInt("playerChoice");
    }

    public void SaveLevelChoice(LevelType levelType)
    {
        PlayerPrefs.SetInt("levelChoice", (int)levelType);
    }

    public LevelType LoadLevelChoice()
    {
        return (LevelType)PlayerPrefs.GetInt("levelChoice");
    }

    public void Activate()
    {
        print("save manager activation");
    }
}
