using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 movementVector;
    float movementSpeed = 5;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movementVector = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movementVector.z += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementVector.z -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementVector.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementVector.x += 1;
        }
    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = movementVector.normalized * movementSpeed;
        if (movementVector != Vector3.zero)
        {
            GetComponent<Rigidbody>().MoveRotation(Quaternion.LookRotation(movementVector));
        }
    }
}
