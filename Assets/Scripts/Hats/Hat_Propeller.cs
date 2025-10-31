using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat_Propeller : MonoBehaviour
{
    float rotationSpeed = 300;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
    }
}
