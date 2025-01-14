using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class MovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 3f;
    public float jumpForce = 5f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public float crouchTransitionSpeed = 5f;

    [Header("Ground Detection")]
    public LayerMask groundLayer;
    public float groundCheckDistance = 1.5f;

    [Header("Camera Settings")]
    public Transform playerCameraTransform;
    public float lookSensitivity = 1f;
    public float smoothing = 1.5f; // Smoothing factor for camera input

    private Gamepad assignedController;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private bool isGrounded;
    private bool isCrouching = false;
    private float currentSpeed;
    private float currentCameraHeight;

    // Camera look variables
    private Vector2 cameraInput;
    private Vector2 velocity;
    private Vector2 frameVelocity;

    //Dev Boolean for Keyboard inputs
    public bool DEVKEYBOARDON;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        if (playerCameraTransform == null)
        {
            Debug.LogError("Player Camera Transform is not assigned in the Inspector!");
        }
        else
        {
            currentCameraHeight = playerCameraTransform.localPosition.y;
        }

        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        if (assignedController == null && DEVKEYBOARDON == false) return;

        HandleMovement();
        HandleCrouchHeight();
        CheckGroundStatus();
        HandleCameraLook();
    }

    public void AssignController(Gamepad controller)
    {
        assignedController = controller;
        Debug.Log($"Controller assigned to {gameObject.name}: {controller.name}");
    }

    private void HandleMovement()
    {
        if (DEVKEYBOARDON == false)
        {
            // Read movement input from the assigned controller
            Vector2 moveInput = assignedController.leftStick.ReadValue();
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

            // Apply movement
            Vector3 worldMoveDirection = transform.TransformDirection(moveDirection);
            Vector3 velocity = new Vector3(worldMoveDirection.x * currentSpeed, rb.velocity.y, worldMoveDirection.z * currentSpeed);
            rb.velocity = velocity;

            // Handle sprint input
            if (assignedController.leftTrigger.isPressed)
                currentSpeed = sprintSpeed;
            else
                currentSpeed = walkSpeed;
        }
        else
        {
            print("DOESNT WORK RN / TURN OFF DEVKEYBOARDON");
/*
            Vector2 moveInput = assignedController.leftStick.ReadValue();
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

            // Apply movement
            Vector3 worldMoveDirection = transform.TransformDirection(moveDirection);
            Vector3 velocity = new Vector3(worldMoveDirection.x * currentSpeed, rb.velocity.y, worldMoveDirection.z * currentSpeed);
            rb.velocity = velocity;

            // Handle sprint input
            if (Input.GetKeyDown(KeyCode.LeftShift))
                currentSpeed = sprintSpeed;
            else
                currentSpeed = walkSpeed;
*/
        }
    }

    private void HandleCrouchHeight()
    {
        if (playerCameraTransform != null)
        {
            float targetHeight = isCrouching ? crouchHeight : standingHeight;
            currentCameraHeight = Mathf.Lerp(currentCameraHeight, targetHeight, Time.deltaTime * crouchTransitionSpeed);

            Vector3 cameraPosition = playerCameraTransform.localPosition;
            cameraPosition.y = currentCameraHeight;
            playerCameraTransform.localPosition = cameraPosition;
        }
    }

    private void ToggleCrouch()
    {
        isCrouching = !isCrouching;
        currentSpeed = isCrouching ? crouchSpeed : walkSpeed;
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    private void HandleCameraLook()
    {
        if (DEVKEYBOARDON == false)
        {
            //Read camera input from the right stick
            Vector2 inputDelta = assignedController.rightStick.ReadValue() * lookSensitivity;

            //Smooth input
            frameVelocity = Vector2.Lerp(frameVelocity, inputDelta, 1 / smoothing);
            velocity += frameVelocity;

            //Clamp vertical look rotation
            velocity.y = Mathf.Clamp(velocity.y, -90f, 90f);

            //Apply rotation to the camera (local X axis for up/down)
            playerCameraTransform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);

            //Apply rotation to the character (Y axis for left/right)
            transform.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        }
        else
        {
            print("DOESNT WORK RN / TURN OFF DEVKEYBOARDON");
            /*
                        Vector2 inputDelta = cameraInput * lookSensitivity;

                        // Smooth input
                        frameVelocity = Vector2.Lerp(frameVelocity, inputDelta, 1 / smoothing);
                        velocity += frameVelocity;

                        // Clamp vertical look rotation
                        velocity.y = Mathf.Clamp(velocity.y, -90f, 90f);

                        // Apply rotation
                        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right); // Up-down rotation
                        transform.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);    // Left-right rotation

                    */
        }
    }
}
