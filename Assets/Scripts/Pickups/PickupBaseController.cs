using UnityEngine;

public enum PickupState
{
    INACTIVE,
    READY,
    ACTIVATED,
}

public class PickupBaseController : MonoBehaviour
{
    GameObject effectChild;

    private Collider pickupCollider;

    PickupState state = PickupState.INACTIVE;

    private void Update()
    {
        PickupStateUpdate();
    }

    void SwitchState(PickupState newState)
    {
        switch (state)
        {
            case PickupState.INACTIVE:
                break;

            case PickupState.READY:
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

            case PickupState.READY:
                break;

            case PickupState.ACTIVATED:
                break;
        }
    }

    public void Setup(GameObject effectChild)
    {
        this.effectChild = effectChild;
        SwitchState(PickupState.READY);
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