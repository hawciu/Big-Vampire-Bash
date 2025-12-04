using UnityEngine;

public class PlayerDamageTrigger : MonoBehaviour
{
    public CapsuleCollider capsuleCollider;

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager.instance.DamagePlayer();
        Debug.Log("Enter: " + other.name + " | Collider: " + other);
    }
    
    public void EnableCollider(bool enable)
    {
        capsuleCollider.enabled = enable;
    }
}
