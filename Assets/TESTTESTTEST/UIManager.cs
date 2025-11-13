using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TMP_Text waveTextNumber;
    public TMP_Text coinsText;

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

    public void UpdateWaveText(string waveText)
    {
        waveTextNumber.text = waveText;
    }

    public void UpdateCoinsText(string coinText)
    {
        coinsText.text = "coins: " + coinText;
    }
}
