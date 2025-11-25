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
    float waitTimeAfterOpening = 1f;
    Vector3 circleTargetScale = new Vector3(4,0,4);
    float characterTargetHeight;
    public ParticleSystem sparks;
    public ParticleSystem portalLines;
    public ParticleSystem end;

    public bool go = true;

    PortalState state = PortalState.NONE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = PortalState.CIRCLING;
        sparks.Play();
        if(guy != null)
        {
            characterTargetHeight = guy.transform.position.y;
        }
        else
        {
            Debug.LogWarning("START portal character guy not set");
        }
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
                    circle.transform.localScale = circleTargetScale * openingCounter / duration + new Vector3(0, 0.01f, 0);
                }
                else
                {
                    portalLines.Play();
                    state = PortalState.TELEPORTING;
                }
                    break;

            case PortalState.TELEPORTING:
                waitTimeAfterOpening -= Time.deltaTime;
                if (waitTimeAfterOpening > 0) return;
                if (characterTargetHeight > -2.5f)
                {
                    characterTargetHeight -= Time.deltaTime * 5;
                    if (guy != null)
                    {
                        guy.transform.position = Vector3.up * characterTargetHeight;
                    }
                    else
                    {
                        Debug.LogWarning("PortalState.TELEPORTING portal character guy not set");
                    }
                }
                else
                {
                    portalLines.Stop();
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
