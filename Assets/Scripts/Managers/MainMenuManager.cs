using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnOpenHubPressed()
    {
        SceneManager.LoadScene("Hub");
    }

    // public void StartLevel()
    // {
    //     SceneManager.LoadScene("Level1");
    // }
}