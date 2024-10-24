using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] private Transform character;  // Character's transform (usually the parent object)
    public float mouseSensitivity = 2f;            // Mouse sensitivity
    public float controllerSensitivity = 1f;       // Controller sensitivity for right stick
    public float smoothing = 1.5f;                 // Smoothing factor for mouse and controller input

    private Vector2 velocity;                      // Current velocity of the rotation
    private Vector2 frameVelocity;                 // Velocity of rotation for this frame

    void Reset()
    {
        character = transform.parent;
    }

    void Start()
    {

    }

    void Update()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 controllerDelta = new Vector2(Input.GetAxisRaw("RightStickHorizontal"), Input.GetAxisRaw("RightStickVertical"));
        Vector2 inputDelta = mouseDelta * mouseSensitivity + controllerDelta * controllerSensitivity;
        frameVelocity = Vector2.Lerp(frameVelocity, inputDelta, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90f, 90f);

        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right); // Up-down rotation
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);    // Left-right rotation
    }
}