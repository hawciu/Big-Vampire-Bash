using UnityEngine;

public class EnemyModelHandler : MonoBehaviour
{
    public GameObject hatTargetObject;
    public GameObject body;
    public GameObject outline;

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
        return body.GetComponent<Renderer>().material;
    }

    public void SetMaterial(Material material)
    {
        body.GetComponent<Renderer>().material = material;
    }

    public void SetMaterialOutline(Material material)
    {
        outline.GetComponent<Renderer>().material = material;
    }
}
