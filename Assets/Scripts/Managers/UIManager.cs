using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TMP_Text isGroundedText;
    public TMP_Text coyoteText;
    public TMP_Text jumpText;
    public TMP_Text movementVectorText;
    public TMP_Text movementSpeedText;
    public TMP_Text isMovingText;

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
        
    }

    public void UpdateIsGroundedText(bool isGrounded)
    {
        isGroundedText.text = isGrounded.ToString();
    }
    public void UpdateCoyoteText(float coyote)
    {
        coyoteText.text = coyote.ToString();
    }
    public void UpdateJumpPeriodText(float jump)
    {
        jumpText.text = jump.ToString();
    }
    public void UpdateMovementVectorText(Vector3 vector)
    {
        movementVectorText.text = vector.ToString();
    }
    public void UpdateMovementSpeedText(float speed)
    {
        movementSpeedText.text = speed.ToString();
    }
    public void UpdateIsMovingText(bool ismoving)
    {
        isMovingText.text = ismoving.ToString();
    }
}
