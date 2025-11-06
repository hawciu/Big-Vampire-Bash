using UnityEngine;

public class PickupWeaponUpgradeEffect : MonoBehaviour
{
    float height = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = PlayerManager.instance.GetPlayer().transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        height += 10 * Time.deltaTime;
        transform.position = PlayerManager.instance.GetPlayer().transform.position + new Vector3(0, height, 0);
        if(transform.position.y >= 5f)
        {
            Destroy(gameObject);
        }
    }
}
