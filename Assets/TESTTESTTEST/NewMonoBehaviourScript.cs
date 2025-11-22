using System;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject circle;
    public GameObject guy;
    float speed = 1f;
    float duration = 0.2f;
    float t = 0f;
    Vector3 targetScale = new Vector3(4,0,4);
    public ParticleSystem end;

    public bool go = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!go) return;
        speed += Time.deltaTime * 5;
        if (speed > 10f)
        {
            speed = 10f;
            circle.SetActive(true);

            if (t < duration)
            {
                t += Time.deltaTime;
                circle.transform.localScale = targetScale * t/duration + new Vector3(0,0.01f,0);
            }
            else
            {
                if(guy.transform.position.y > -2.5f)
                {
                    guy.transform.position += Vector3.up * -Time.deltaTime * 5;
                }
                else
                {
                    go = false;
                    Destroy(guy);
                    Destroy(circle);
                    end.Play();
                    Destroy(gameObject);
                }
            }
        }
        transform.Rotate(0, 100 * Time.deltaTime * speed, 0);
    }
}
