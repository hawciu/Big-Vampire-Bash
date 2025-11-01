using UnityEngine;

public class PickupController : MonoBehaviour
{
    private PickupDataScriptableObject data;
    private GameObject modelInstance;

    public void Setup(PickupDataScriptableObject newData)
    {
        data = newData;
        if (data.pickupModelPrefab != null)
        {
            modelInstance = Instantiate(data.pickupModelPrefab, transform);
            modelInstance.transform.localPosition = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            data.Activate(other.gameObject);
            Destroy(gameObject);
        }
    }
}
