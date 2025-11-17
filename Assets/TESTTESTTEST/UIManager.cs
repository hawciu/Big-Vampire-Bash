using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TMP_Text waveTextNumber;
    public TMP_Text coinsText;

    public GameObject IngameMenu;
    public GameObject GameOverScreen;
    public TMP_Text enemiesCountText;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        enemiesCountText.text = "zywe moby: " + EnemyManager.instance.aliveEnemies;
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
}