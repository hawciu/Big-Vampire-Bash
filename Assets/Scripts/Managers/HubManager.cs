using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubManager : MonoBehaviour
{
    public static HubManager instance;
    public Transform previewParent;

    public List<GameObject> playerPrefabs;

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
        if (currentIndex >= playerPrefabs.Count)
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
            currentIndex = playerPrefabs.Count - 1;
        }

        ShowPreview(currentIndex);
    }

    private void ShowPreview(int index)
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }

        currentPreview = Instantiate(playerPrefabs[index], previewParent);

        currentPreview.transform.localPosition = Vector3.zero;
        currentPreview.transform.localRotation = Quaternion.identity;
        currentPreview.transform.localScale = Vector3.one;
    }

    public int GetSelectedIndex()
    {
        return currentIndex;
    }

    public void StartLevel()
    {
        SceneManager.LoadScene("Level1");
    }
}