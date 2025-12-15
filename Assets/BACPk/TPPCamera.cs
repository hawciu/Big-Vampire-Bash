using UnityEngine;
using UnityEngine.InputSystem;

public class TPPCamera : MonoBehaviour
{
    public Transform target;
    public float sensitivity = 1.5f;
    public float distance = 4f;
    public Vector2 pitchLimits = new Vector2(-40, 70);

    private PlayerInputActions input;
    private float yaw;
    private float pitch;

    void Awake()
    {
        input = new PlayerInputActions();
    }

    void OnEnable() => input.Enable();
    void OnDisable() => input.Disable();

    void LateUpdate()
    {
        Vector2 look = input.Player.Look.ReadValue<Vector2>();

        // Mouse vs Controller scaling
        float multiplier = Mouse.current != null ? 0.1f : 1f;

        yaw += look.x * sensitivity * multiplier;
        pitch -= look.y * sensitivity * multiplier;
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.rotation = rotation;
        transform.position = target.position + Vector3.up * 2 - rotation * Vector3.forward * distance;
    }
}