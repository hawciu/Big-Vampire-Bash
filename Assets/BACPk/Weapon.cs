using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public CapsuleCollider weaponCollider;
    public EnemyWeaponController enemyWeaponController;


    internal void DamageStart()
    {
        weaponCollider.enabled = true;
    }

    internal void DamageEnd()
    {
        weaponCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("weapon hit " + other.gameObject.name);
        enemyWeaponController.TryAttacking(other.gameObject);
    }
}
