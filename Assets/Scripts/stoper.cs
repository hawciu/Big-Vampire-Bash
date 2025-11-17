using TMPro;
using UnityEngine;

public class stoper : MonoBehaviour
{
    public TMP_Text text;

    private float startTime = 0;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        float time = Time.time - startTime;

        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)((time - (int)time) * 100);

        text.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}