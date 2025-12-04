using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public GameObject playerFollowCameraPrefab;
    Vector3 landscapeCameraOffset;
    Vector3 portraitCameraOffset;
    int lastScreenWidth;
    bool isLandscapeMode;
    Camera playerCamera;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastScreenWidth = Screen.width;
        isLandscapeMode = Screen.width > Screen.height;
        NotifyScreenOrientation();
    }

    // Update is called once per frame
    void Update()
    {
        ScreenRotationCheck();
    }

    private void ScreenRotationCheck()
    {
        if (Screen.width == lastScreenWidth) return;
        lastScreenWidth = Screen.width;
        OnScreenOrientationChange();
        NotifyScreenOrientation();
    }

    private void OnScreenOrientationChange()
    {

    }

    void NotifyScreenOrientation()
    {
        if (isLandscapeMode)
        {
            Debug.Log("screen is in LANDSCAPE mode");
        }
        else
        {
            Debug.Log("screen is in PORTRAIT mode");
        }
    }

    public void SpawnPlayerCamera()
    {
        GameObject tmp =  Instantiate(playerFollowCameraPrefab);
        playerCamera = tmp.GetComponent<Camera>();
    }

    public Camera GetPlayerCamera()
    {
        return playerCamera;
    }
}
