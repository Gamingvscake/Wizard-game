using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] private Transform character;  // Character's transform (usually the parent object)
    public float lookSensitivity = 1f;            // Sensitivity multiplier for right stick
    public float smoothing = 1.5f;                // Smoothing factor for input

    private PlayerController controls;              // Input action map
    private Vector2 cameraInput;                  // Input vector for camera movement
    private Vector2 velocity;                     // Current velocity of rotation
    private Vector2 frameVelocity;                // Velocity of rotation for this frame

    void Awake()
    {
        controls = new PlayerController(); // Initialize input system
        controls.PlayerControls.CameraControl.performed += ctx => cameraInput = ctx.ReadValue<Vector2>();
        controls.PlayerControls.CameraControl.canceled += ctx => cameraInput = Vector2.zero;
    }

    void OnEnable()
    {
        controls.PlayerControls.Enable();
    }

    void OnDisable()
    {
        controls.PlayerControls.Disable();
    }

    void Reset()
    {
        character = transform.parent; // Default character reference
    }

    void Update()
    {
        // Apply input sensitivity
        Vector2 inputDelta = cameraInput * lookSensitivity;

        // Smooth input
        frameVelocity = Vector2.Lerp(frameVelocity, inputDelta, 1 / smoothing);
        velocity += frameVelocity;

        // Clamp vertical look rotation
        velocity.y = Mathf.Clamp(velocity.y, -90f, 90f);

        // Apply rotation
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right); // Up-down rotation
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);    // Left-right rotation
    }
}