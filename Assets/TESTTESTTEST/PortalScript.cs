using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public enum PortalState
{
    NONE,
    MOVE_CAMERA_TO_PORTAL,
    CIRCLING,
    OPENING,
    TELEPORTING,
    CLOSING,
    MOVE_CAMERA_BACK,
}

public class PortalScript : MonoBehaviour
{
    public Camera mainCamera;

    public Camera cinematicCamera;
    public GameObject center;
    public GameObject circle;
    public GameObject guy;
    float speed = 1f;
    float duration = 0.2f;
    float openingCounter = 0f;
    float waitTimeAfterOpening = 0.5f;
    float cameraTransitionCounter = 0f;
    Vector3 cameraTargetPosition;
    Quaternion cameraTargetRotation;
    Vector3 startCameraTargetPosition;
    Quaternion startCameraTargetRotation;
    public GameObject portalCameraTarget;
    Vector3 circleTargetScale = new Vector3(4,0,4);
    float characterTargetHeight;
    public ParticleSystem sparks;
    public ParticleSystem portalLines;
    public ParticleSystem end;
    public bool enteringPortal = false;
    bool rotate = false;
    bool skipMoveTo = false;

    PortalState state = PortalState.NONE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //to remove upon implementation
        mainCamera = Camera.main;
        SetupPortal(guy, mainCamera);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePortalState();
    }

    public void SetupPortal(GameObject guyTarget, Camera targetCamera, bool enteringPorta = true, bool skipMoveTo = false)
    {
        guy = guyTarget;
        mainCamera = targetCamera;
        this.skipMoveTo = skipMoveTo;
        if (enteringPortal)
        {
            cinematicCamera.transform.position = mainCamera.transform.position;
            cinematicCamera.transform.rotation = mainCamera.transform.rotation;
            cameraTargetPosition = portalCameraTarget.transform.position;
            cameraTargetRotation = portalCameraTarget.transform.rotation;
        }
        else
        {
            cinematicCamera.transform.position = portalCameraTarget.transform.position;
            cinematicCamera.transform.rotation = portalCameraTarget.transform.rotation;
            cameraTargetPosition = mainCamera.transform.position;
            cameraTargetRotation = mainCamera.transform.rotation;
        }
        startCameraTargetPosition = portalCameraTarget.transform.position;
        startCameraTargetRotation = portalCameraTarget.transform.rotation;
        ActivatePortal();
    }

    public void ActivatePortal()
    {
        mainCamera.enabled = false;
        cinematicCamera.enabled = true;
        rotate = true;
        sparks.Play();
        if (guy != null)
        {
            characterTargetHeight = 1.4f;
        }
        else
        {
            Debug.LogWarning("START portal character guy not set");
        }
        state = PortalState.MOVE_CAMERA_TO_PORTAL;
    }

    private void UpdatePortalState()
    {
        if (rotate) center.transform.Rotate(0, 100 * Time.deltaTime * speed, 0);
        switch (state)
        {
            case PortalState.NONE:
                break;

            case PortalState.MOVE_CAMERA_TO_PORTAL:
                if (skipMoveTo || MoveCameraLerp())
                {
                    state = PortalState.CIRCLING;
                }
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
                    circle.transform.localScale = circleTargetScale * openingCounter / duration;
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
                state = PortalState.MOVE_CAMERA_BACK;
                break;

            case PortalState.MOVE_CAMERA_BACK:
                if (MoveCameraLerp())
                {
                    state = PortalState.NONE;
                    PortalActionCompleted();
                }
                break;
        }
    }

    bool MoveCameraLerp()
    {
        cameraTransitionCounter += Time.deltaTime;
        cinematicCamera.transform.position = Vector3.Lerp(startCameraTargetPosition, cameraTargetPosition, cameraTransitionCounter);
        cinematicCamera.transform.rotation = Quaternion.Lerp(startCameraTargetRotation, cameraTargetRotation, cameraTransitionCounter);
        return cameraTransitionCounter >= 1;
    }

    void PortalActionCompleted()
    {

    }
}
