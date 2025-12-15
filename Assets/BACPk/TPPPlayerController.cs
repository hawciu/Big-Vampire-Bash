using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    NONE,
    MOVING,
    JUMPING,
    ATTACKING,
}


[RequireComponent(typeof(CharacterController))]
public class TPPPlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float sprintSpeed = 9f;
    public float rotationSpeed = 10f;

    [Header("Jump")]
    public float jumpHeight = 1.5f;
    public float gravity = -20;//9.81f;
    public float coyoteTime = 0.15f;      // grace after leaving ground
    public float jumpBufferTime = 0.15f;  // grace before landing
    private const float groundStickForce = -0.1f;

    private float coyoteTimer;
    private float jumpBufferTimer = 1;

    private CharacterController controller;
    private PlayerInputActions input;
    Collider[] hitInfo = new Collider[1];
    public Animator animator;

    Vector3 movementDirection;

    Vector3 movementVector;

    bool isGrounded;
    bool isMoving;
    bool isSprinting;
    public PlayerState state = PlayerState.NONE;
    private bool tryLanding;
    bool canCancelAttack = false;
    bool canCombo = false;
    float attackCombo = 0;

    public SwordScript swordScript;

    void OnEnable() => input.Enable();
    void OnDisable() => input.Disable();

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = new PlayerInputActions();
    }

    private void Start()
    {
        SwitchState(PlayerState.MOVING);
    }

    void Update()
    {
        UpdateStateMachine();
        UpdateUIDebug();
    }

    void SwitchState(PlayerState newState)
    {
        state = newState;
        switch (state)
        {
            case PlayerState.NONE:
                break;

            case PlayerState.MOVING:
                animator.SetBool("isJumping", false);
                break;

            case PlayerState.JUMPING:
                tryLanding = false;
                animator.SetBool("isJumping", true);
                break;

            case PlayerState.ATTACKING:
                animator.SetFloat("attackCombo", attackCombo);
                animator.SetTrigger("attack");
                break;
        }
    }

    private void UpdateStateMachine()
    {
        switch (state)
        {
            case PlayerState.NONE:
                break;

            case PlayerState.MOVING:
                AttackCheck();
                HandleMovement();
                HandleJump();
                ApplyGravity();
                UpdateMovement();
                UpdateAnimator();
                break;

            case PlayerState.JUMPING:
                AttackCheck();
                HandleMovement();
                HandleJump();
                ApplyGravity();
                UpdateMovement();
                if (tryLanding && isGrounded) SwitchState(PlayerState.MOVING);
                break;

            case PlayerState.ATTACKING:
                if (canCancelAttack)
                {
                    AttackCheck();
                    if (input.Player.Move.ReadValue<Vector2>().magnitude > 0)
                    {
                        SwitchState(PlayerState.MOVING);
                    }
                }
                break;
        }
    }

    private void AttackCheck()
    {
        if (input.Player.Attack.WasPressedThisFrame())
        {
            if (canCombo)
            {
                NextAttackCombo();
            }
            canCancelAttack = false;
            canCombo = false;
            SwitchState(PlayerState.ATTACKING);
        }
    }

    void UpdateUIDebug()
    {
        UIManager.instance.UpdateCoyoteText(coyoteTimer);
        UIManager.instance.UpdateJumpPeriodText(jumpBufferTimer);
        UIManager.instance.UpdateIsGroundedText(isGrounded);
    }

    private void UpdateAnimator()
    {
        Vector2 velocityVector = new Vector2(controller.velocity.x, controller.velocity.z);
        animator.SetBool("isMoving", isMoving);
        UIManager.instance.UpdateIsMovingText(isMoving);
        float speedSmooth = velocityVector.magnitude;
        speedSmooth  = Mathf.Lerp(speedSmooth, 1, Time.deltaTime / 0.1f);
        animator.SetFloat("movementSpeed", velocityVector.magnitude/9);
        UIManager.instance.UpdateMovementSpeedText(velocityVector.magnitude/9);
    }

    void HandleMovement()
    {
        isGrounded = IsGrounded();

        Vector2 moveInput = input.Player.Move.ReadValue<Vector2>();

        isMoving = moveInput.magnitude > 0;
        isSprinting = input.Player.Sprint.IsPressed();

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Camera-relative movement
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = camRight.y = 0;

        movementDirection = (camForward * move.z + camRight * move.x);
        if(movementDirection.magnitude > 1) movementDirection = movementDirection.normalized;

        if (movementDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        float speed = isSprinting ? sprintSpeed : moveSpeed;
        movementDirection *= speed;
        movementVector.x = movementDirection.x;
        movementVector.z = movementDirection.z;
    }

    void HandleJump()
    {
        // Track grounded grace time
        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        // Track jump buffer
        if (input.Player.Jump.WasPressedThisFrame())
        {
            jumpBufferTimer = jumpBufferTime;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        // Execute jump
        if (input.Player.Jump.WasPressedThisFrame() && coyoteTimer > 0)
        {
            movementVector.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpBufferTimer = 0;
            coyoteTimer = 0;
            SwitchState(PlayerState.JUMPING);
        }
    }

    void ApplyGravity()
    {
        if (!isGrounded) movementVector.y += gravity * Time.deltaTime;
        if (movementVector.y < gravity) movementVector.y = gravity;
    }

    bool IsGrounded()
    {
        int hits = Physics.OverlapSphereNonAlloc(
            transform.position + Vector3.down * 0.1f,
            controller.radius * 0.8f,
            hitInfo,
            LayerMask.GetMask("Default")
        );

        return hits > 0;
    }

    void UpdateMovement()
    {
        UIManager.instance.UpdateMovementVectorText(movementVector);
        controller.Move(movementVector * Time.deltaTime);
    }

    internal void CheckForGround()
    {
        tryLanding = true;
    }

    internal void CanCancelAttack()
    {
        canCombo = true;
        canCancelAttack = true;
    }

    internal void AttackEnd()
    {
        animator.SetTrigger("attackEnd");
        SwitchState(PlayerState.MOVING);
    }

    void NextAttackCombo()
    {
        attackCombo = (attackCombo == 0 || attackCombo == 2) ? 1 : 2;
    }

    public void ComboWindowEnd()
    {
        canCombo = false;
        attackCombo = 0;
    }

    public void ActivateWeaponDamage(bool ifActivate)
    {
        swordScript.ActivateWeaponDamage(ifActivate);
    }
}