using System.Collections;
using UnityEngine;

public class PickupBaseController : MonoBehaviour
{
    [Header("Spawn sequence")]
    [SerializeField] private GameObject preEffectChild;

    [SerializeField] private float preEffectDuration = 2f;

    [Header("Pickup visual")]
    public GameObject effectChild;

    private Collider pickupCollider;
    private bool isReadyToPickup = false;

    private void Awake()
    {
        pickupCollider = GetComponent<Collider>();

        if (pickupCollider != null)
        {
            pickupCollider.enabled = false;
        }

        if (preEffectChild != null)
        {
            preEffectChild.SetActive(false);
        }

        if (effectChild != null)
        {
            effectChild.SetActive(false);
        }
    }

    private void Start()
    {
        _ = StartCoroutine(SpawnSequence());
    }

    private IEnumerator SpawnSequence()
    {
        if (preEffectChild != null)
        {
            preEffectChild.SetActive(true);
        }

        yield return new WaitForSeconds(preEffectDuration);

        if (preEffectChild != null)
        {
            preEffectChild.SetActive(false);
        }

        if (effectChild != null)
        {
            effectChild.SetActive(true);
        }

        if (pickupCollider != null)
        {
            pickupCollider.enabled = true;
        }

        isReadyToPickup = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isReadyToPickup)
        {
            return;
        }

        isReadyToPickup = false;

        if (pickupCollider != null)
        {
            pickupCollider.enabled = false;
        }

        if (effectChild != null)
        {
            IPickupEffect effect = effectChild.GetComponent<IPickupEffect>();
            effect?.Activate();
        }

        EffectsManager.instance.SpawnAnEffect(
            ParticleType.PICKUP_PICKUP,
            transform.position
        );
    }

    public void KillPickup()
    {
        Destroy(gameObject);
    }
}