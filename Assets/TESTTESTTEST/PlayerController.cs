using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 movementVector;
    private readonly float movementSpeed = 12;
    public Animator animator;

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
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
        if (movementVector == Vector3.zero)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
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
