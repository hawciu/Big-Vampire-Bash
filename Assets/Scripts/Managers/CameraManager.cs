using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public GameObject playerFollowCameraPrefab;
    Vector3 landscapeCameraOffset = new Vector3(0, 12.5f, -9f);
    Vector3 portraitCameraOffset = new Vector3(0, 35.5f, -22f);
    Vector3 currentCameraOffset;
    int lastScreenWidth;
    bool isLandscapeMode;
    Camera playerCamera;
    PlayerFollowCamera playerFollowCamera;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ScreenRotationCheck();
    }

    public void ScreenRotationCheck()
    {
        if (Screen.width == lastScreenWidth) return;
        lastScreenWidth = Screen.width;
        OnScreenOrientationChange();
    }

    private void OnScreenOrientationChange()
    {
        isLandscapeMode = Screen.width > Screen.height;
        UpdateCameraOffset();
    }

    public void SpawnPlayerCamera()
    {
        GameObject tmp =  Instantiate(playerFollowCameraPrefab);
        playerCamera = tmp.GetComponent<Camera>();
        playerFollowCamera = tmp.GetComponent<PlayerFollowCamera>();
    }

    public Camera GetPlayerCamera()
    {
        return playerCamera;
    }

    void UpdateCameraOffset()
    {
        currentCameraOffset = isLandscapeMode ? landscapeCameraOffset : portraitCameraOffset;
    }

    public Vector3 GetCurrentCameraOffset()
    {
        return currentCameraOffset;
    }

    public void SwitchCameraState(CameraState state)
    {
        playerFollowCamera.SwitchCameraState(state);
    }

    public void UpdatePlayerFollowCamera()
    {
        playerFollowCamera.UpdateCameraState();
    }
}
