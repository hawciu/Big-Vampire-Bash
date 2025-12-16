using System;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public CapsuleCollider capsuleCollider;
    List<GameObject> targetsHitThisSwing = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void ActivateWeaponDamage(bool ifActivate)
    {
        capsuleCollider.enabled = ifActivate;
        targetsHitThisSwing.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if (!targetsHitThisSwing.Contains(other.gameObject))
        {
            targetsHitThisSwing.Add(other.gameObject);
            other.gameObject.TryGetComponent<IDamageable>(out IDamageable component);
            component.Damage(1);
        }
    }
}
