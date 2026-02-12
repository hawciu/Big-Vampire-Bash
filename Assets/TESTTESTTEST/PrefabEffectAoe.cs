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

    PrefabEffectState state = PrefabEffectState.INACTIVE;


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
                pickupEffect.Play();
                break;

            case PrefabEffectState.CLEANUP:
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
                //get enemies in range
                //kill
                //wait 1s then hide visual indicator
                //go to cleanup
                break;

            case PrefabEffectState.CLEANUP:
                break;

        }
    }

    public void MakeReady()
    {
        SwitchState(PrefabEffectState.READY);
    }

    public void Activate()
    {
        SwitchState(PrefabEffectState.ACTIVE);
    }
}
