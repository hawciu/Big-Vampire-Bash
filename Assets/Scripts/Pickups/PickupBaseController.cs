using UnityEngine;

public class PickupBaseController : MonoBehaviour
{
    public GameObject effectChild;

    private Collider pickupCollider;

    private void Awake()
    {
        pickupCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pickupCollider != null)
        {
            pickupCollider.enabled = false;
        }

        if (effectChild != null)
        {
            EffectsManager.instance.SpawnAnEffect(ParticleType.PICKUP_PICKUP, transform.position);
            IPickupEffect effect = effectChild.GetComponent<IPickupEffect>();
            effect?.Activate();
        }
    }
}