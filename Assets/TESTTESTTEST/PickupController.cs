using UnityEngine;

public class PickupController : MonoBehaviour
{
    public GameObject effectChild;

    private Collider pickupCollider;

    private void Awake()
    {
        pickupCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (pickupCollider != null)
        {
            pickupCollider.enabled = false;
        }

        if (effectChild != null)
        {
            IPickupEffect effect = effectChild.GetComponent<IPickupEffect>();
            effect?.Activate(other.gameObject);
        }
    }
}