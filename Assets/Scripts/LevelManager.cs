using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public GameObject wall;

    float bounds = 35;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update japa
    void Start()
    {
        Vector3 nw = new Vector3(-bounds, 0, bounds);
        Vector3 ne = new Vector3(bounds, 0, bounds);
        Vector3 sw = new Vector3(-bounds, 0, -bounds);
        Vector3 se = new Vector3(bounds, 0, -bounds);
        BuildWall(nw, ne);
        BuildWall(ne, se);
        BuildWall(se, sw);
        BuildWall(sw, nw);
    }
    
    void BuildWall(Vector3 target, Vector3 rot)
    {
        GameObject tmp;
        tmp = Instantiate(wall, target, Quaternion.identity);
        tmp.transform.localScale = new Vector3(tmp.transform.localScale.x,
            tmp.transform.localScale.y,
            bounds*2);//tmp.transform.localScale.z);
        tmp.transform.rotation = Quaternion.LookRotation(rot - tmp.transform.position);
        tmp.transform.position += (rot-target).normalized * bounds;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetBounds()
    {
        return bounds;
    }
}
