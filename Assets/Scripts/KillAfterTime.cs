using UnityEngine;

public class KillAfterTime : MonoBehaviour
{
    float startTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime + 0.5f < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
