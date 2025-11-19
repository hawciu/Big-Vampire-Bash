using UnityEngine;

public class EnemyModelHandler : MonoBehaviour
{
    public GameObject hatTargetObject;
    public GameObject model;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Material GetMaterial()
    {
        return model.GetComponent<Renderer>().material;
    }

    public void SetMaterial(Material material)
    {
        model.GetComponent<Renderer>().material = material;
    }
}
