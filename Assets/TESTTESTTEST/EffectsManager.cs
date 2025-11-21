using System.Collections.Generic;
using UnityEngine;
public enum ParticleType
{
    ARROW_EXPLOSION,
    PICKUP_PICKUP,


}

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager instance;

    [System.Serializable]
    public struct ParticleEntry
    {
        public ParticleType type;
        public ParticleSystem prefab;
    }

    public ParticleEntry[] entries;
    private Dictionary<ParticleType, ParticleSystem> effectsDictionary = new();

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

    public void SpawnAnEffect(ParticleType type, Vector3 location)
    {
        Instantiate(effectsDictionary[type], location, Quaternion.identity);
    }
}
