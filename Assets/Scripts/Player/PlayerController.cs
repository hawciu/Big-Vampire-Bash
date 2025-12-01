using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPausable
{
    private Vector3 movementVector;
    private readonly float movementSpeed = 12;
    public Animator animator;
    public Rigidbody rb;
    Vector3 storedMovementVector;

    private void Update()
    {
        UpdateControlsInput();
    }

    private void FixedUpdate()
    {
        UpdatePlayerMovement();
    }

    private void UpdatePlayerMovement()
    {
        rb.linearVelocity = movementVector.normalized * movementSpeed;
        if (movementVector != Vector3.zero)
        {
            rb.MoveRotation(Quaternion.LookRotation(movementVector));
        }

    }

    void UpdateControlsInput()
    {
        movementVector = Vector3.zero;
        if (!PlayerManager.instance.GetPlayerControlsEnabled()) return;
        
        if (Input.GetKey(KeyCode.W))
        {
            movementVector.z += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementVector.z -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementVector.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementVector.x += 1;
        }
        if (movementVector == Vector3.zero)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }
    }

    public void SetPlayerAnimator(Animator animator)
    {
        this.animator = animator;
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            animator.speed = 0;
        }
        else
        {
            animator.speed = 1;
        }
    }
}