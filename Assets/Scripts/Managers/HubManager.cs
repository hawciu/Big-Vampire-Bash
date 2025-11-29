using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubManager : MonoBehaviour
{
    public static HubManager instance;
    public Transform previewParent;

    private int currentIndex = 0;
    private GameObject currentPreview;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ShowPreview(currentIndex);
    }

    public void Next()
    {
        currentIndex++;
        if (currentIndex >= EnemyDatabaseManager.instance.PlayersObjects.Count)
        {
            currentIndex = 0;
        }

        ShowPreview(currentIndex);
    }

    public void Previous()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = EnemyDatabaseManager.instance.PlayersObjects.Count - 1;
        }

        ShowPreview(currentIndex);
    }

    private void ShowPreview(int index)
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }

        currentPreview = Instantiate(EnemyDatabaseManager.instance.PlayersObjects[index].playerModelPrefab, previewParent);
    }

    public int GetSelectedIndex()
    {
        return currentIndex;
    }

    public void StartLevel()
    {
        SaveManager.instance.SavePlayerChoice((PlayerType)currentIndex);
        SaveManager.instance.SaveLevelChoice(0); //placeholder level choice
        PlayerPrefs.Save();
        SceneManager.LoadScene("Level1");
    }
}