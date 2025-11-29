using System.Collections.Generic;
using UnityEngine;


public class MaterialManager : MonoBehaviour
{
    public static MaterialManager instance;

    [System.Serializable]
    public struct MaterialEntry
    {
        public EnemyMaterialType type;
        public Material mat;
    }

    public MaterialEntry[] entries;
    private Dictionary<EnemyMaterialType, Material> materialsDictionary = new();

    private void Awake()
    {
        instance = this;
        //should be in activation, if it ever gets implemented
        foreach (MaterialEntry e in entries)
            materialsDictionary[e.type] = e.mat;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Material GetMaterial(EnemyMaterialType type)
    {
        return materialsDictionary[type];
    }
}
