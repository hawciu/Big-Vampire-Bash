using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 movementVector;
    private readonly float movementSpeed = 12;
    public Animator animator;
    public Rigidbody rb;

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
        if (PlayerManager.instance == null || !PlayerManager.instance.GetPlayerControlsEnabled()) return;
        
        movementVector = Vector3.zero;
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
}