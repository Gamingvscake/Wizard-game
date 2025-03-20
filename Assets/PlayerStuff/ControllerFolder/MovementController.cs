using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class MovementController : MonoBehaviour
{
    [Header("DEV TOOLS")]
    public bool DevKeyboardOn;

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
    public float smoothing = 5f; // Smoothing factor for camera rotation

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
    public Animator animator;

    // Shooting
    public bool ShootSpell;

    [Header("Throw Settings")]
    public GameObject objectToThrow; // Prefab to throw
    public Transform throwPoint; // The spawn position of the object (should be child of camera)
    public Transform playerCamera; // Assign the player's camera here
    public float throwForce = 10f; // Strength of the throw
    public float randomTorque = 10f; // Random rotation force
    public int numberOfAvailablePotions; 
    public int maxNumberOfPotions;

    [Header("Audio")]
    [SerializeField] private AudioSource playersteps;

    public float timer = 0f;
    public float timelimit = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator = GetComponentInChildren<Animator>();
        numberOfAvailablePotions = 1;
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
        if (assignedController == null && DevKeyboardOn == false) return;

        HandleMovement();
        HandleCrouchHeight();
        CheckGroundStatus();
        HandleCameraLook();
        HandleShooting();
    }

    public void HandleShooting()
    {
        if (!DevKeyboardOn)
        {
            if (assignedController.rightTrigger.isPressed == true)
            {
                ShootSpell = true;
            }
            else
            {
                ShootSpell = false;
            }
        }
    }

    public void AssignController(Gamepad controller)
    {
        assignedController = controller;
        Debug.Log($"Controller assigned to {gameObject.name}: {controller.name}");
    }

    private void HandleMovement()
    {
        if (DevKeyboardOn == false)
        {
            // Read movement input from the assigned controller
            Vector2 moveInput = assignedController.leftStick.ReadValue();
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

            // Check if the player is moving
            bool isMoving = moveInput.magnitude > 0.1f;

            // Apply movement
            Vector3 worldMoveDirection = transform.TransformDirection(moveDirection);
            Vector3 velocity = new Vector3(worldMoveDirection.x * currentSpeed, rb.velocity.y, worldMoveDirection.z * currentSpeed);
            rb.velocity = velocity;

            // Handle sprint input
            bool isSprinting = assignedController.leftStickButton.isPressed;

            if (assignedController.buttonEast.wasPressedThisFrame && numberOfAvailablePotions >0)
            {
                //ToggleCrouch();
                Throw();
            }
            if (assignedController.buttonSouth.wasPressedThisFrame)
            {
                Jump();
                animator.SetTrigger("Jumping");
            }

            if (isSprinting)
                currentSpeed = sprintSpeed;
            else
                currentSpeed = walkSpeed;

            // Set Animator Parameters
            animator.SetBool("Walking", isMoving);
            animator.SetBool("Sprinting", isSprinting && isMoving);
            animator.SetBool("Grounded", isGrounded);

            // Play footsteps sound only if the player is moving
            if (isMoving)
            {
                if (timer >= timelimit)
                {
                    if (!playersteps.isPlaying) // Only play if it's not already playing
                    {
                        playersteps.Play();
                    }

                    timer = 0f;
                }
                timer += Time.deltaTime;
            }
            else
            {
                if (playersteps.isPlaying) // Stop sound when not moving
                {
                    playersteps.Stop();
                }
            }
        }
        else
        {
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            Vector3 worldMoveDirection = transform.TransformDirection(moveDirection);
            if (timer >= timelimit)
            {
                playersteps.Play();
                timer = 0f;
            }
            timer += Time.deltaTime;

            // Apply velocity, keeping the y velocity (gravity and jumping) intact
            Vector3 velocity = new Vector3(worldMoveDirection.x * currentSpeed, rb.velocity.y, worldMoveDirection.z * currentSpeed);
            rb.velocity = velocity;

            // Handle sprint input
            if (Input.GetKey(KeyCode.LeftShift))
                currentSpeed = sprintSpeed;
            else
                currentSpeed = walkSpeed;
            if (Input.GetKeyDown(KeyCode.G) && numberOfAvailablePotions > 0)
            {
                Throw();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                ToggleCrouch();
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                ToggleCrouch();
            }

            // Stop the sound if the player stops moving
            if (moveDirection.magnitude < 0.1f)
            {
                if (playersteps.isPlaying)
                {
                    playersteps.Stop();
                }
            }
        }
    }

    private void HandleCameraLook()
    {
        if (DevKeyboardOn == false)
        {
            // Read camera input from the right stick
            Vector2 inputDelta = assignedController.rightStick.ReadValue() * lookSensitivity;

            // Apply rotation input directly to velocity
            velocity.x += inputDelta.x;
            velocity.y -= inputDelta.y;

            // Smooth the rotation transition before applying it
            Vector2 smoothedVelocity = Vector2.Lerp(velocity, new Vector2(velocity.x, velocity.y), Time.deltaTime * smoothing);

            // Clamp the Y-axis (vertical look) after smoothing to prevent exceeding limits
            smoothedVelocity.y = Mathf.Clamp(smoothedVelocity.y, -90f, 90f);

            // Apply rotation to the camera (local X axis for up/down)
            playerCameraTransform.localRotation = Quaternion.Euler(smoothedVelocity.y, 0f, 0f);

            // Apply rotation to the character (Y axis for left/right)
            transform.localRotation = Quaternion.Euler(0f, smoothedVelocity.x, 0f);

            // Update velocity with the clamped/smoothed value to ensure consistent state
            velocity = smoothedVelocity;
        }
        else
        {
            float tempx = Input.GetAxis("Mouse X");
            float tempy = Input.GetAxis("Mouse Y");
            cameraInput = new Vector2(tempx, tempy);
            Vector2 inputDelta = cameraInput * (lookSensitivity * 2);

            // Apply rotation to the camera immediately, without smoothing on inputDelta
            velocity.x += inputDelta.x;
            velocity.y -= inputDelta.y;

            // Smooth the rotation transition over time
            velocity = Vector2.Lerp(velocity, new Vector2(velocity.x, Mathf.Clamp(velocity.y, -90f, 90f)), Time.deltaTime * smoothing);

            // Apply rotation to the camera (local X axis for up/down)
            playerCameraTransform.localRotation = Quaternion.Euler(velocity.y, 0f, 0f);

            // Apply rotation to the character (Y axis for left/right)
            transform.localRotation = Quaternion.Euler(0f, velocity.x, 0f);
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

    void Throw()
    {
        numberOfAvailablePotions -= 1;
        GameObject thrownObject = Instantiate(objectToThrow, throwPoint.position, Quaternion.identity);

        Rigidbody rb = thrownObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Thrown object needs a Rigidbody!");
            return;
        }

        Vector3 throwDirection = playerCamera.forward;

        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        Vector3 randomTorqueVector = new Vector3(
            Random.Range(-randomTorque, randomTorque),
            Random.Range(-randomTorque, randomTorque),
            Random.Range(-randomTorque, randomTorque)
        );
        rb.AddTorque(randomTorqueVector, ForceMode.Impulse);
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
}