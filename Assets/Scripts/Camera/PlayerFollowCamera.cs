using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    FOLLOW_PLAYER,
    ZOOM_PLAYER,
}

public class PlayerFollowCamera : MonoBehaviour
{
    CameraState state = CameraState.FOLLOW_PLAYER;
    private float cameraTransitionCounter;
    private float characterTransitionCounterCurve;
    public AnimationCurve animationCurve;
    Vector3 startCameraPosition;
    Vector3 targetCameraPosition;
    Quaternion startCameraRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraState();
    }

    public void UpdateCameraState()
    {
        switch (state)
        {
            case CameraState.FOLLOW_PLAYER:
                gameObject.transform.position = PlayerManager.instance.GetPlayer().transform.position + CameraManager.instance.GetCurrentCameraOffset();
                break;

            case CameraState.ZOOM_PLAYER:
                cameraTransitionCounter += Time.deltaTime / 10;
                characterTransitionCounterCurve = animationCurve.Evaluate(cameraTransitionCounter);
                transform.position = Vector3.Lerp(startCameraPosition,
                    PlayerManager.instance.GetPlayerCameraZoomTarget().transform.position, characterTransitionCounterCurve);
                transform.rotation = Quaternion.Lerp(startCameraRotation,
                    PlayerManager.instance.GetPlayerCameraZoomTarget().transform.rotation, characterTransitionCounterCurve);
                if(cameraTransitionCounter > 0.15f)
                {
                    PlayerManager.instance.GetPlayerCameraZoomTargetPivot().transform.Rotate(Vector3.up, 5*Time.deltaTime);
                }
                break; 
        }
    }

    public void SwitchCameraState(CameraState newState)
    {
        switch (newState)
        {
            case CameraState.FOLLOW_PLAYER:
                break;

            case CameraState.ZOOM_PLAYER:
                cameraTransitionCounter = 0;
                state = CameraState.ZOOM_PLAYER;
                startCameraPosition = transform.position;
                startCameraRotation = transform.rotation;
                break;
        }
    }
}
