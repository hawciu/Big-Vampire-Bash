using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;


enum PortalState
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
    float characterTransitionCounter = 0f;
    float characterTransitionCounterCurve = 0f;
    float characterStartHeight;
    Vector3 cameraTargetPosition;
    Quaternion cameraTargetRotation;
    Vector3 startCameraTargetPosition;
    Quaternion startCameraTargetRotation;
    public GameObject portalCameraTarget;
    Vector3 circleTargetScale = new Vector3(4,0,4);
    float characterCurrentHeight;
    float characterTargetHeight;
    float characterHeightLow = -3f;
    float characterHeightHigh = 0;
    public ParticleSystem sparks;
    public ParticleSystem portalLines;
    public ParticleSystem end;
    bool rotate = false;
    bool noZoomIn = false;
    bool noZoomOut = false;
    public PortalFunction portalFunction = PortalFunction.ENTER_NOZOOMIN;

    public AnimationCurve animationCurve = new();

    PortalState state = PortalState.NONE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePortalState();
    }

    public void SetupPortal(GameObject guyTarget, Camera targetCamera, PortalFunction function)
    {
        guy = guyTarget;
        mainCamera = targetCamera;
        portalFunction = function;
        switch (portalFunction)
        {
            case PortalFunction.ENTER_NOZOOMIN:
                noZoomIn = true;
                noZoomOut = true;
                startCameraTargetPosition = mainCamera.transform.position;
                startCameraTargetRotation = mainCamera.transform.rotation;
                cameraTargetPosition = portalCameraTarget.transform.position;
                cameraTargetRotation = portalCameraTarget.transform.rotation;
                cinematicCamera.transform.position = portalCameraTarget.transform.position;
                cinematicCamera.transform.rotation = portalCameraTarget.transform.rotation;
                break;

            case PortalFunction.ENTER:
                noZoomIn = false;
                noZoomOut = true;
                startCameraTargetPosition = mainCamera.transform.position;
                startCameraTargetRotation = mainCamera.transform.rotation;
                cameraTargetPosition = portalCameraTarget.transform.position;
                cameraTargetRotation = portalCameraTarget.transform.rotation;
                cinematicCamera.transform.position = mainCamera.transform.position;
                cinematicCamera.transform.rotation = mainCamera.transform.rotation;
                break;

            case PortalFunction.LEAVE:
                noZoomIn = true;
                noZoomOut = false;
                startCameraTargetPosition = portalCameraTarget.transform.position;
                startCameraTargetRotation = portalCameraTarget.transform.rotation;
                cameraTargetPosition = mainCamera.transform.position;
                cameraTargetRotation = mainCamera.transform.rotation;
                cinematicCamera.transform.position = portalCameraTarget.transform.position;
                cinematicCamera.transform.rotation = portalCameraTarget.transform.rotation;
                break;
        }
        ActivatePortal();
    }

    public void ActivatePortal()
    {
        mainCamera.enabled = false;
        cinematicCamera.enabled = true;
        rotate = true;
        characterCurrentHeight = guy.transform.position.y;
        if (portalFunction == PortalFunction.LEAVE)
        {
            guy.transform.position = new Vector3(guy.transform.position.x, characterHeightLow, guy.transform.position.z);
            characterTargetHeight = characterHeightHigh;
        }
        else
        {
            guy.transform.position = new Vector3(guy.transform.position.x, characterHeightHigh, guy.transform.position.z);
            characterTargetHeight = characterHeightLow;
        }
        characterStartHeight = guy.transform.position.y;
        sparks.Play();
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
                if (noZoomIn || MoveCameraLerp())
                {
                    cameraTransitionCounter = 0;
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
                characterTransitionCounter += Time.deltaTime * 3;
                if(characterTransitionCounter > 1) characterTransitionCounter = 1;
                guy.transform.position = new Vector3(guy.transform.position.x,
                    Mathf.Lerp(characterStartHeight, characterTargetHeight, characterTransitionCounter),
                    guy.transform.position.z);
                if (characterTransitionCounter >= 1)
                {
                    portalLines.Stop();
                    state = PortalState.CLOSING;
                    EffectsManager.instance.SpawnAnEffect(ParticleType.PORTAL_LEAVE_EFFECT, PlayerManager.instance.GetPlayer().transform.position, new Vector3(0,1,0));
                }
                break;

            case PortalState.CLOSING:
                circle.SetActive(false);
                end.Play();
                center.SetActive(false);
                state = PortalState.MOVE_CAMERA_BACK;
                break;

            case PortalState.MOVE_CAMERA_BACK:
                if (noZoomOut || MoveCameraLerp())
                {
                    cameraTransitionCounter = 0;
                    state = PortalState.NONE;
                    PortalActionCompleted();
                }
                break;
        }
    }

    bool MoveCameraLerp()
    {
        cameraTransitionCounter += Time.deltaTime;
        characterTransitionCounterCurve = animationCurve.Evaluate(cameraTransitionCounter);
        cinematicCamera.transform.position = Vector3.Lerp(startCameraTargetPosition, cameraTargetPosition, characterTransitionCounterCurve);
        cinematicCamera.transform.rotation = Quaternion.Lerp(startCameraTargetRotation, cameraTargetRotation, characterTransitionCounterCurve);
        return cameraTransitionCounter >= 1;
    }

    void PortalActionCompleted()
    {
        mainCamera.enabled = true;
        cinematicCamera.enabled = false;
        GameManager.instance.OnPortalActionCompleted(portalFunction);
        Destroy(gameObject);
    }
}
