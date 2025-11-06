using UnityEngine;

public class PickupWeaponUpgradeBase : MonoBehaviour, IPickupEffect
{
    public GameObject pickupModel;
    public GameObject pickupEffect;

    public void Activate()
    {
        PlayerManager.instance.UpgradePlayerWeapon();
        Instantiate(pickupEffect);
        Destroy(gameObject);
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
