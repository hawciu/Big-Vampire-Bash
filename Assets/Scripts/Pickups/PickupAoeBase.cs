using UnityEngine;

public class PickupAoeBase : MonoBehaviour, IPickupEffect
{
    public GameObject visualIndicator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(GameObject player)
    {
        float range = 10;
        GameObject tmp = Instantiate(visualIndicator, transform.position, Quaternion.identity);
        tmp.transform.localScale = new Vector3(2*range, 0.01f, 2*range);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("ENEMY"));
        foreach (Collider hitCollider in hitColliders)
        {
            hitCollider.gameObject.GetComponent<EnemySimple>().Damage();
        }
        Destroy(transform.parent.gameObject);
    }
}
