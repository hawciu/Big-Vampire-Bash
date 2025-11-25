using System;
using UnityEngine;

public enum PortalState
{
    NONE,
    CIRCLING,
    OPENING,
    TELEPORTING,
    CLOSING
}

public class PortalScript : MonoBehaviour
{
    public GameObject center;
    public GameObject circle;
    public GameObject guy;
    float speed = 1f;
    float duration = 0.2f;
    float openingCounter = 0f;
    Vector3 targetScale = new Vector3(4,0,4);
    public ParticleSystem sparks;
    public ParticleSystem end;

    public bool go = true;

    PortalState state = PortalState.NONE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = PortalState.CIRCLING;
        sparks.Play();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePortalState();
        if(state != PortalState.NONE) center.transform.Rotate(0, 100 * Time.deltaTime * speed, 0);
    }

    private void UpdatePortalState()
    {
        switch (state)
        {
            case PortalState.NONE:
                break;

            case PortalState.CIRCLING:
                speed += Time.deltaTime * 5;
                if (speed > 10f)
                {
                    speed = 10f;
                    circle.SetActive(true);
                    openingCounter = 0;
                    state = PortalState.OPENING;
                }
                break;

            case PortalState.OPENING:
                if (openingCounter < duration)
                {
                    openingCounter += Time.deltaTime;
                    circle.transform.localScale = targetScale * openingCounter / duration + new Vector3(0, 0.01f, 0);
                }
                else
                {
                    state = PortalState.TELEPORTING;
                }
                    break;

            case PortalState.TELEPORTING:
                if (guy.transform.position.y > -2.5f)
                {
                    guy.transform.position += Vector3.up * -Time.deltaTime * 5;
                }
                else
                {
                    state = PortalState.CLOSING; 
                }
                break;

            case PortalState.CLOSING:
                circle.SetActive(false);
                end.Play();
                center.SetActive(false);
                state = PortalState.NONE;
                break;
        }
    }
}
