using UnityEngine;

public class PlayerDamageTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerManager.instance.DamagePlayer();
        Debug.Log("Enter: " + other.name + " | Collider: " + other);
    }
}
