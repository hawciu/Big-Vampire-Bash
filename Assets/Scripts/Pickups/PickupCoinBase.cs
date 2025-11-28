using UnityEngine;

public class PickupCoinBase : MonoBehaviour, IPickupEffect
{
    public GameObject pickupModel;

    public void Activate()
    {
        GameManager.instance.OnCoinPickup();
        EffectsManager.instance.SpawnAnEffect(ParticleType.COIN_PICKUP, transform.position);
        Destroy(transform.parent.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
