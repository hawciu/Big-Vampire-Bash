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
    GameObject effectChild;
    ParticleSystem rippleEffect;

    private Collider pickupCollider;

    PickupState state = PickupState.INACTIVE;
    float spawningStart;
    float spawnTime = 2f;

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
                spawningStart = Time.time;
                break;

            case PickupState.READY:
                rippleEffect.Stop();

                effectChild.GetComponent<IPickupEffect>().MakeReady();
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
                spawningStart += Time.deltaTime;
                if (spawningStart + spawnTime > Time.time)
                {
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
        Destroy(gameObject);
    }
}