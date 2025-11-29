using UnityEngine;

public class KillAfterTime : MonoBehaviour
{
    float startTime;
    public float killAfterSeconds = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime + killAfterSeconds < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
