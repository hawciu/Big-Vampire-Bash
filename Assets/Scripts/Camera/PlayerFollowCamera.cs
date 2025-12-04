using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    Vector3 cameraFollowOffset = new Vector3(0, 20, -7);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerManager.instance.GetPlayer().transform.position + cameraFollowOffset;
    }
}
