using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintMultiplier = 2f;

    [Header("Jump")]
    public float jumpHeight = 3f;
    public bool enableJump = true;

    [Header("Crouch")]
    public float crouchHeight = 0.5f;
    public bool holdToCrouch = true;
    public float crouchSpeedReduction = 0.5f;
    private float originalHeight;

    [Header("Head Bob")]
    public bool enableHeadBob = true;
    public Transform cameraJoint;
    public float bobSpeed = 14f;
    public Vector3 bobAmount = new Vector3(0.1f, 0.1f, 0);

    private CharacterController characterController;
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private float speed;
    private bool isCrouching = false;
    private bool isSprinting = false;

    private float verticalVelocity = 0f;
    private float gravity = -9.81f;
    private bool isGrounded;

    private Vector3 headBobPosition;
    private float bobTimer = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        originalHeight = characterController.height;
        speed = walkSpeed;

        // Bind Input Actions
        playerInput.actions["Movement"].performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Movement"].canceled += ctx => moveInput = Vector2.zero;

        playerInput.actions["Sprint"].performed += ctx => isSprinting = true;
        playerInput.actions["Sprint"].canceled += ctx => isSprinting = false;

        playerInput.actions["Crouch"].performed += ctx => HandleCrouch(ctx);
        playerInput.actions["Jump"].performed += ctx => HandleJump();
    }

    void Update()
    {
        HandleMovement();
        HandleHeadBob();
    }

    void HandleMovement()
    {
        isGrounded = characterController.isGrounded;

        // Calculate movement direction
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        // Adjust speed for crouching and sprinting
        float currentSpeed = walkSpeed;
        if (isSprinting && !isCrouching) currentSpeed *= sprintMultiplier;
        if (isCrouching) currentSpeed *= crouchSpeedReduction;

        characterController.Move(move * currentSpeed * Time.deltaTime);

        // Apply gravity
        if (isGrounded && verticalVelocity < 0) verticalVelocity = -2f;
        verticalVelocity += gravity * Time.deltaTime;

        characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    void HandleJump()
    {
        if (enableJump && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void HandleCrouch(InputAction.CallbackContext ctx)
    {
        if (!holdToCrouch)
        {
            isCrouching = !isCrouching;
        }
        else
        {
            isCrouching = ctx.performed;
        }

        characterController.height = isCrouching ? crouchHeight : originalHeight;
    }

    void HandleHeadBob()
    {
        if (!enableHeadBob || moveInput == Vector2.zero) return;

        bobTimer += Time.deltaTime * bobSpeed;
        float bobAmountY = Mathf.Sin(bobTimer) * bobAmount.y;
        float bobAmountX = Mathf.Cos(bobTimer / 2) * bobAmount.x;

        headBobPosition = new Vector3(bobAmountX, bobAmountY, 0);
        cameraJoint.localPosition = Vector3.Lerp(cameraJoint.localPosition, headBobPosition, Time.deltaTime * 10f);
    }
}