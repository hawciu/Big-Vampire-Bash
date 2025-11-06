using UnityEngine;
using TMPro;

public class stoper : MonoBehaviour
{
    public TMP_Text text;

    float startTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float time;
        time = Time.time - startTime;
        text.text = ((int)(time / 60)) + ":" + ((time - (int)(time / 60)*60).ToString().Split(".")[0]);
    }
}
