using UnityEngine;

public class PlayerAnimEventHandler : MonoBehaviour
{
    public TPPPlayerController tppPlayerController;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckForGround()
    {
        tppPlayerController.CheckForGround();
    }

    public void CanCancelAttack()
    {
        tppPlayerController.CanCancelAttack();
    }

    public void AttackEnd()
    {
        tppPlayerController.AttackEnd();
    }

    public void ComboWindowEnd()
    {
        tppPlayerController.ComboWindowEnd();
    }
}
