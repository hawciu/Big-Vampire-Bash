using UnityEngine;

public class EnemyModelHandler : MonoBehaviour
{
    public GameObject hatTargetObject;
    public GameObject model;
    public GameObject outlineTarget;

    public Material GetMaterial()
    {
        return model.GetComponent<Renderer>().material;
    }
}