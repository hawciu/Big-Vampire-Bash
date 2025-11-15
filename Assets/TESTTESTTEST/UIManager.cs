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
        IngameMenu.SetActive(!IngameMenu.activeSelf);
        GameManager.instance.PauseGame(IngameMenu.activeSelf);
    }

    public void OnGameOver()
    {
        GameOverScreen.SetActive(true);
    }

    public void OnQuitButtonPressed()
    {
        GameManager.instance.PauseGame(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void OnRestartButtonPressed()
    {
        GameManager.instance.PauseGame(false);
        SceneManager.LoadScene("Level1");
    }

    public void OnContinueButtonPressed()
    {

    }
}
