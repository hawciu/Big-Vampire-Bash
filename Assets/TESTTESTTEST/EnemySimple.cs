using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimple : MonoBehaviour
{
    float speed = 1;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;/*
        Vector3 moveDirection = PlayerManager.instance.GetPlayer().transform.position - transform.position;
        moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
        GetComponent<Rigidbody>().MovePosition(transform.position + moveDirection.normalized * speed * Time.deltaTime);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().MoveRotation(Quaternion.LookRotation(moveDirection.normalized));
    */
    }
}
