using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager instance;

    [Header ("Loose prefabs")]
    public GameObject PortalPrefab;
    public GameObject EndGamePortalPrefab;

    [System.Serializable]
    public struct ParticleEntry
    {
        public ParticleType type;
        public GameObject prefab;
    }

    [Header("Dictionary effects")]
    public ParticleEntry[] entries;
    private Dictionary<ParticleType, GameObject> effectsDictionary = new();

    private void Awake()
    {
        instance = this;
        //should be in activation, if it ever gets implemented
        foreach (ParticleEntry e in entries)
            effectsDictionary[e.type] = e.prefab;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnAnEffect(ParticleType particleType, Vector3 location, Vector3 offset)
    {
        Instantiate(effectsDictionary[particleType], location + offset, Quaternion.identity);
    }

    public void SpawnAnEffect(ParticleType particleType, Vector3 location)
    {
        SpawnAnEffect(particleType, location, Vector3.zero);
    }

    public GameObject SpawnPortal(Vector3 location)
    {
        return Instantiate(PortalPrefab, location, Quaternion.identity);
    }

    public GameObject SpawnEndGamePortal(Vector3 location)
    {
        return Instantiate(EndGamePortalPrefab, location, Quaternion.identity);
    }
}
