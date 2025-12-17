using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public Enemy enemyscript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print("detected "+other.gameObject.name);
        enemyscript.PlayerSpotted(other.gameObject);
    }
}
