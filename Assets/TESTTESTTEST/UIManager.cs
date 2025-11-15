using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TMP_Text waveTextNumber;
    public TMP_Text coinsText;

    public GameObject IngameMenu;
    public GameObject GameOverScreen;

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

    public void OnIngameMenuButtonPressed()
    {
        GameManager.instance.PauseGame(IngameMenu.activeSelf);
        IngameMenu.SetActive(!IngameMenu.activeSelf);
    }

    public void OnGameOver()
    {
        GameOverScreen.SetActive(true);
    }

    public void OnQuitButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnContinueButtonPressed()
    {

    }
}
