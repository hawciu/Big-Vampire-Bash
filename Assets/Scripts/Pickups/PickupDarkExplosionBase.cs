using UnityEngine;

public class PickupDarkExplosionBase : MonoBehaviour, IPickupEffect
{
    public GameObject PickupDarkExplosion;
    public GameObject PickupDarkExplosionCollectableFX;

    public void Activate()
    {
        GameObject player = PlayerManager.instance.GetPlayer();


        if (PickupDarkExplosionCollectableFX != null)
        {
            PickupDarkExplosionCollectableFX.SetActive(false);
        }


        if (player != null && PickupDarkExplosion != null)
        {
            Vector3 spawnPos = player.transform.position + new Vector3(0f, 0.1f, 0f);
            _ = Instantiate(PickupDarkExplosion, spawnPos, Quaternion.identity);
        }
    }
}