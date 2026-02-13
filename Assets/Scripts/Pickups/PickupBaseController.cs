using System;
using UnityEngine;

public enum PickupState
{
    INACTIVE,
    SPAWNING,
    READY,
    ACTIVATED,
}

public class PickupBaseController : MonoBehaviour
{
    PickupState state = PickupState.INACTIVE;
    
    GameObject effectChild;

    public ParticleSystem rippleEffect;
    public ParticleSystem rippleEffectOnce;

    public Collider pickupCollider;

    float spawningStart;
    float spawnTime = 5f;
    float spawnEmissionRateMax = 3f;
    float spawnTimeLerpCounter = 0;
    public AnimationCurve emissionRateCurve;

    ParticleSystem.EmissionModule emission;

    private void Update()
    {
        PickupStateUpdate();
    }

    void SwitchState(PickupState newState)
    {
        this.state = newState;
        switch (state)
        {
            case PickupState.INACTIVE:
                break;

            case PickupState.SPAWNING:
                rippleEffect.Play();
                emission = rippleEffect.emission;
                spawningStart = Time.time;
                break;

            case PickupState.READY:
                rippleEffect.Stop();
                rippleEffectOnce.Play();

                pickupCollider.enabled = true;

                effectChild.GetComponent<IPickupEffect>().MakeReady(this);
                break;

            case PickupState.ACTIVATED:
                break;
        }
    }

    void PickupStateUpdate()
    {
        switch (state)
        {
            case PickupState.INACTIVE:
                break;

            case PickupState.SPAWNING:
                spawnTimeLerpCounter += Time.deltaTime;
                emission.rateOverTime = emissionRateCurve.Evaluate(spawnTimeLerpCounter / spawnTime) * spawnEmissionRateMax;
                if (spawningStart + spawnTime < Time.time)
                {
                    rippleEffectOnce.Play();
                    SwitchState(PickupState.READY);
                }
                break;

            case PickupState.READY:
                break;

            case PickupState.ACTIVATED:
                break;
        }
    }

    public void Setup(GameObject effectChild)
    {
        this.effectChild = effectChild;
        SwitchState(PickupState.SPAWNING);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state != PickupState.READY) return;

        pickupCollider.enabled = false;

        EffectsManager.instance.SpawnAnEffect(ParticleType.PICKUP_PICKUP, transform.position);
        IPickupEffect effect = effectChild.GetComponent<IPickupEffect>();
        effect?.Activate();

        SwitchState(PickupState.ACTIVATED);
    }

    public void DeactivatePickup()
    {
        Destroy(effectChild);
        Destroy(gameObject);
    }

    internal void OnPrefabEffectFinished()
    {
        DeactivatePickup();
    }
}