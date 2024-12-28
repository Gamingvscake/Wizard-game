using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 3f;
    public float jumpForce = 5f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f; // Lower camera height when crouching
    public float standingHeight = 2f; // Default camera height
    public float crouchTransitionSpeed = 5f; // Speed of camera height transition

    [Header("Ground Detection")]
    public LayerMask groundLayer; // Layer mask to check for ground collisions
    public float groundCheckDistance = 1.5f; // Distance for the raycast to check

    [Header("Camera Settings")]
    public Transform playerCameraTransform; // Reference to the camera's transform, assigned via the Inspector

    private PlayerController controls; // Input action asset reference
    private Vector2 movementInput;
    private bool isGrounded;
    private bool isCrouching = false;
    private float currentSpeed;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    // Variable to track the camera's current height
    private float currentCameraHeight;

    private void Awake()
    {
        controls = new PlayerController(); // Initialize the input system

        controls.PlayerControls.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        controls.PlayerControls.Movement.canceled += _ => movementInput = Vector2.zero;

        controls.PlayerControls.Jump.performed += _ => Jump();
        controls.PlayerControls.Crouch.performed += _ => ToggleCrouch();
        controls.PlayerControls.Sprint.performed += _ => currentSpeed = sprintSpeed;
        controls.PlayerControls.Sprint.canceled += _ => currentSpeed = walkSpeed;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        // Ensure the camera transform is assigned
        if (playerCameraTransform == null)
        {
            Debug.LogError("Player Camera Transform is not assigned in the Inspector!");
        }
        else
        {
            currentCameraHeight = playerCameraTransform.localPosition.y; // Start with the camera's initial height
        }

        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        Move();
        CheckGroundStatus();
        HandleCrouchHeight();
    }

    private void Move()
    {
        // Get movement direction and convert it to world space
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        Vector3 worldMoveDirection = transform.TransformDirection(moveDirection);

        // Apply velocity, keeping the y velocity (gravity and jumping) intact
        Vector3 velocity = new Vector3(worldMoveDirection.x * currentSpeed, rb.velocity.y, worldMoveDirection.z * currentSpeed);
        rb.velocity = velocity;
    }

    private void Jump()
    {
        // Ensure the player can only jump if they are grounded
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); // Apply jump force
        }
    }

    private void ToggleCrouch()
    {
        // Toggle crouch state and adjust speed accordingly
        isCrouching = !isCrouching;
        currentSpeed = isCrouching ? crouchSpeed : walkSpeed;
    }

    private void HandleCrouchHeight()
    {
        // Smoothly transition between crouch and standing height for the camera
        if (playerCameraTransform != null)
        {
            float targetHeight = isCrouching ? crouchHeight : standingHeight;
            currentCameraHeight = Mathf.Lerp(currentCameraHeight, targetHeight, Time.deltaTime * crouchTransitionSpeed);

            // Update the camera's local position to match the desired height
            Vector3 cameraPosition = playerCameraTransform.localPosition;
            cameraPosition.y = currentCameraHeight;
            playerCameraTransform.localPosition = cameraPosition;
        }
    }

    private void CheckGroundStatus()
    {
        // Raycast downwards from the player's position to check if they're grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        // Optional: Debug the ground check to visualize in the Scene view
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }

    private void OnEnable()
    {
        controls.PlayerControls.Enable();
    }

    private void OnDisable()
    {
        controls.PlayerControls.Disable();
    }
}