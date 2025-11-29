using UnityEngine;

public class PlayerDamageTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerManager.instance.DamagePlayer();
        print("trigger enter " + other.gameObject.name);
    }
}
