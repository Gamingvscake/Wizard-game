using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class MovementController : MonoBehaviour
{
    /*public TMP_Text[] reviveTexts;
    private static Dictionary<int, int> reviveCounts = new Dictionary<int, int>();
    public ReviveScript revivescripts;
    public GameObject revivestuff;*/

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
    private Vector2 rotation;
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
    public GameObject fullPotionIcon;

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
        if (numberOfAvailablePotions > 0 && fullPotionIcon.activeInHierarchy == false) fullPotionIcon.SetActive(true);
        else if (numberOfAvailablePotions <= 0 && fullPotionIcon.activeInHierarchy == true) fullPotionIcon.SetActive(false);
        HandleMovement();
        HandleCrouchHeight();
        CheckGroundStatus();
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

    public void FixedUpdate()
    {
        HandleCameraLook();
    }

    private void HandleMovement()
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

    private void HandleCameraLook()
    {
        // Read right stick input
        Vector2 inputDelta = assignedController.rightStick.ReadValue() * lookSensitivity;

        // Accumulate input into rotation
        rotation.x += inputDelta.x;
        rotation.y -= inputDelta.y;

        // Clamp the vertical (pitch) rotation
        rotation.y = Mathf.Clamp(rotation.y, -90f, 90f);

        // Apply rotation to the camera (X for pitch, Y for yaw)
        playerCameraTransform.localRotation = Quaternion.Euler(rotation.y, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, rotation.x, 0f);
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
        thrownObject.GetComponent<FlaskExploder>().reviverObject = this.gameObject;
        /*revivestuff.GetComponent<ReviveScript>().reviveTexts = reviveTexts;*/
        
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