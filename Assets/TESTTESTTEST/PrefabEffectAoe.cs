using System;
using UnityEngine;

public enum PrefabEffectState
{
    INACTIVE,
    READY,
    ACTIVE,
    CLEANUP
}

public class PrefabEffectAoe : MonoBehaviour, IPickupEffect
{
    public GameObject visualEffect;
    public ParticleSystem pickupEffect;
    public GameObject rangeVisualIndicator;
    public LayerMask enemyLayer;

    PrefabEffectState state = PrefabEffectState.INACTIVE;

    float aoeRange = 5f;
    float activationStart;
    PickupBaseController parentPickupBaseController;

    private void Update()
    {
        PrefabStateUpdate();
    }

    void SwitchState(PrefabEffectState state)
    {
        this.state = state;
        switch (state)
        {
            case PrefabEffectState.INACTIVE:
                break;

            case PrefabEffectState.READY:
                visualEffect.SetActive(true);
                break;

            case PrefabEffectState.ACTIVE:
                visualEffect.SetActive(false);
                rangeVisualIndicator.SetActive(true);
                rangeVisualIndicator.transform.localScale = new Vector3(1, 0.1f, 1) * aoeRange * 2;
                pickupEffect.Play();
                Collider[] hits = Physics.OverlapSphere(transform.position, aoeRange, enemyLayer);
                foreach (Collider hit in hits)
                {
                    if (hit.TryGetComponent<EnemySimple>(out EnemySimple enemy))
                    {
                        enemy.Damage();
                    }
                }
                activationStart = Time.time;
                print("activated: " + activationStart);
                break;

            case PrefabEffectState.CLEANUP:
                rangeVisualIndicator.SetActive(false);
                break;

        }
    }

    private void PrefabStateUpdate()
    {
        switch (state)
        {
            case PrefabEffectState.INACTIVE:
                break;

            case PrefabEffectState.READY:
                break;

            case PrefabEffectState.ACTIVE:
                if (activationStart + 0.5f < Time.time)
                {
                    SwitchState(PrefabEffectState.CLEANUP);
                }
                break;

            case PrefabEffectState.CLEANUP:
                OnPrefabEffectFinished();
                break;

        }
    }

    public void MakeReady(PickupBaseController parent)
    {
        parentPickupBaseController = parent;
        SwitchState(PrefabEffectState.READY);
    }

    public void Activate()
    {
        SwitchState(PrefabEffectState.ACTIVE);
    }

    public void OnPrefabEffectFinished()
    {
        parentPickupBaseController.OnPrefabEffectFinished();
    }
}
