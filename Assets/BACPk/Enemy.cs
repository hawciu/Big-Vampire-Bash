using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public enum EnemyBacparState
{
    IDLE,
    MOVING,
    ATTACKING,
    STUN,
    DEAD,
}

public class Enemy : MonoBehaviour, IDamageable
{
    int health = 3;
    public EnemyBacparState state = EnemyBacparState.IDLE;
    private CharacterController controller;
    public Animator animator;

    Vector3 movementVector;

    public GameObject target;
    public EnemyWeaponController weaponController;

    float chaseRange = 20;
    float attackRange = 2;
    float lastAttack = 0;
    float stunTimer = 0;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        EnemyStateHandler();
    }

    void SwitchState(EnemyBacparState value)
    {
        print("switch state " + value.ToString());
        state = value;
        switch (state)
        {
            case EnemyBacparState.IDLE:
                animator.SetBool("isMoving", false);
                break;

            case EnemyBacparState.MOVING:
                animator.SetBool("isMoving", true);
                break;

            case EnemyBacparState.ATTACKING:
                animator.SetTrigger("isAttacking");
                break;

            case EnemyBacparState.STUN:
                animator.SetTrigger("isStunned");
                stunTimer = 5;
                break;

            case EnemyBacparState.DEAD:
                break;
        }
    }

    void EnemyStateHandler()
    {
        switch (state)
        {
            case EnemyBacparState.IDLE:
                break;

            case EnemyBacparState.MOVING:
                HandleMovingState();
                break;

            case EnemyBacparState.ATTACKING:
                break;

            case EnemyBacparState.STUN:
                stunTimer -= Time.deltaTime;
                if (stunTimer <= 0)
                {
                    SwitchState(EnemyBacparState.MOVING);
                    weaponController.DamageEnd();
                }
                break;

            case EnemyBacparState.DEAD:
                break;
        }
    }

    private void HandleMovingState()
    {
        print("target null check");
        if (target == null)
        {
            SwitchState(EnemyBacparState.IDLE);
            return;
        }
        MoveAndTurn();
        TargetDistanceCheck();
    }

    private void MoveAndTurn()
    {
        print("move and turn");
        movementVector = -(gameObject.transform.position - target.transform.position).normalized;
        controller.Move(movementVector * Time.deltaTime);
        Quaternion targetRotation = Quaternion.LookRotation(movementVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime
        );
    }

    private void TargetDistanceCheck()
    {
        print("target distance check");
        float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);
        if (distance < chaseRange)
        {
            print("< chase range");
            if (distance < attackRange)
            {
                if (lastAttack + 1 > Time.time) return;
                print("< attackRange range");
                animator.SetBool("isMoving", false);
                print("target in range, attacking");
                SwitchState(EnemyBacparState.ATTACKING);
                return;
            }
            else print("> attack range");
        }
        else
        {
            print("target out of range");
            target = null;
            animator.SetBool("isMoving", false);
            SwitchState(EnemyBacparState.IDLE);
            return;
        }
    }

    public void AttackEnd()
    {
        print("attack end");
        SwitchState(EnemyBacparState.MOVING);
    }

    public void Damage(int amount)
    {
        health = health - amount;
        CheckIfDead();
    }

    private void CheckIfDead()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    internal void PlayerSpotted(GameObject playerTarget)
    {
        target = playerTarget;
        SwitchState(EnemyBacparState.MOVING);
    }

    internal void DamageStart()
    {
        weaponController.DamageStart(); Stun();
    }

    internal void DamageEnd()
    {
        weaponController.DamageEnd();
    }

    public bool IsStunned()
    {
        return state == EnemyBacparState.STUN;
    }

    public void Stun()
    {
        SwitchState(EnemyBacparState.STUN);
    }
}
