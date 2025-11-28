using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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

    public void Activate()
    {
        print("save manager activation");
    }
}
