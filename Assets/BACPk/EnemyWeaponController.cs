using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    public List<Weapon> weaponsList = new();
    List<GameObject> targetsHitThisSwing = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DamageStart()
    {
        targetsHitThisSwing.Clear();
        foreach (Weapon weapon in weaponsList)
        {
            weapon.DamageStart();
        }
    }

    public void DamageEnd()
    {
        foreach (Weapon weapon in weaponsList)
        {
            weapon.DamageEnd();
        }
    }

    internal void TryAttacking(GameObject targetGameObject)
    {
        if (!targetsHitThisSwing.Contains(targetGameObject))
        {
            targetsHitThisSwing.Add(targetGameObject);
            if (targetGameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.Damage(1);
            }
        }
    }
}
