using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public Vector3 moveDirection;
    Vector3 startLocation;
    float speed = 11;

    // Start is called before the first frame update
    void Start()
    {
        startLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(startLocation, transform.position) > 50) Destroy(gameObject);
        GetComponent<Rigidbody>().MovePosition(transform.position + moveDirection.normalized * Time.deltaTime * speed);
    }

    public void Setup(Vector3 direction)
    {
        moveDirection = direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject);
        if (other.gameObject == PlayerManager.instance.GetPlayer()) return;
        if(other.gameObject.TryGetComponent<EnemySimple>(out EnemySimple t))
        {
            t.Kill();
        }
        Destroy(gameObject);
    }
}
