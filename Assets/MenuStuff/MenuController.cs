using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class TriggerAnimationWithCameraMove : MonoBehaviour
{
    [Tooltip("Animation Settings")]
    public Animator animator;
    public string triggerName = "Interact";

    [Tooltip("Camera Movement Settings")]
    public Camera mainCamera; // Reference to the main camera
    public Transform targetTransform; // Target position and rotation for the camera
    public float cameraMoveDelay = 1.0f; // Time to wait before moving the camera
    public float cameraMoveDuration = 2.0f; // Time it takes to move the camera smoothly

    [Tooltip("Selection Settings")]
    public List<TextMeshProUGUI> MainMenuTexts;
    public List<TextMeshProUGUI> SettingsMenuTexts;
    public List<TextMeshProUGUI> CurrentMenuTexts;
    public Color defaultOutlineColor = new Color(0, 0, 0, 0); // No outline by default
    public Color selectedOutlineColor = new Color(215, 215, 215); // White outline for selected text
    public float pulseSpeed = 2.0f; // Speed of the pulsing effect
    public float maxOutlineWidth = 0.3f; // Maximum outline width during the pulse
    private int selectionIndex = 0; // Current selected index

    [Tooltip("Screen Objects")]
    public GameObject MenuScreen;
    public GameObject SettingsScreen;
    public GameObject GeneralSettings;
    public GameObject AudioSettings;
    public GameObject ControlsSettings;


    private bool isMovingCamera = false;
    public bool BookOpen = false;
    public bool MainScreenOpen = true;
    public bool SettingsScreenOpen = false;
    public bool GeneralSettingsOpen = false;
    public bool AudioSettingsOpen = false;
    public bool ControlsSettingsOpen = false;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        CurrentMenuTexts = MainMenuTexts;
        UpdateTextOutlines(); // Ensure the correct text is highlighted on start
        MenuScreen.SetActive(true);
        SettingsScreen.SetActive(false);
        GeneralSettings.SetActive(false);
        AudioSettings.SetActive(false);
        ControlsSettings.SetActive(false);
        MainScreenOpen = true;
    }

    void Update()
    {
        // Trigger animation and camera movement
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger(triggerName);
            StartCoroutine(MoveCameraAfterDelay());
        }

        // Handle selection index updates
        HandleSelectionInput();
        MenuControls();
    }

    private bool canMove = true; // Track if input is allowed
    private bool stickInUse = false; // Track if the stick is currently in use

    private void HandleSelectionInput()
    {
        // Check for input (LeftStick or DPad)
        float verticalInput = Input.GetAxis("Vertical"); // Left stick vertical input
        bool dpadUp = Input.GetKeyDown(KeyCode.W); // D-Pad up
        bool dpadDown = Input.GetKeyDown(KeyCode.S); // D-Pad down

        // Handle stick movement
        if (!stickInUse && Mathf.Abs(verticalInput) > 0.5f)
        {
            stickInUse = true;

            if (verticalInput > 0)
            {
                ChangeSelection(-1); // Move up in the list
            }
            else if (verticalInput < 0)
            {
                ChangeSelection(1); // Move down in the list
            }
        }

        // Reset stickInUse when stick returns to neutral
        if (Mathf.Abs(verticalInput) < 0.1f)
        {
            stickInUse = false;
        }

        // Handle DPad input (debounced)
        if (canMove)
        {
            if (dpadUp)
            {
                ChangeSelection(-1); // Move up in the list
                canMove = false; // Disable movement until button released
            }
            else if (dpadDown)
            {
                ChangeSelection(1); // Move down in the list
                canMove = false; // Disable movement until button released
            }
        }

        // Reset canMove when DPad buttons are no longer pressed
        if (!dpadUp && !dpadDown)
        {
            canMove = true;
        }
    }

    private void MenuControls()
    {
        if (BookOpen && Input.GetButtonDown("Jump"))
        {
            if (MainScreenOpen)
            {
                if (selectionIndex == 0)
                {
                    SceneManager.LoadScene(2);
                }
                if (selectionIndex == 1)
                {
                    SettingsScreenOpen = true;
                    CurrentMenuTexts = SettingsMenuTexts;
                    ChangeSelection(-1);
                    UpdateTextOutlines();
                    SettingsScreen.SetActive(true);
                    MenuScreen.SetActive(false);
                    MainScreenOpen = false;
                }
            }
            if (SettingsScreenOpen)
            {
                if (selectionIndex == 3)
                {
                    MainScreenOpen = true;
                    CurrentMenuTexts = MainMenuTexts;
                    ChangeSelection(1);
                    UpdateTextOutlines();
                    MenuScreen.SetActive(true);
                    SettingsScreen.SetActive(false);
                    SettingsScreenOpen = false;
                }
            }

        }
    }

    private void ChangeSelection(int direction)
    {
        // Update selection index and wrap around

        if (SettingsScreenOpen)
        {
            selectionIndex = (selectionIndex + direction + 4) % 4; //wraps selection number around 0-3
        }
        else
        {
            selectionIndex = (selectionIndex + direction + 3) % 3; // Modulo ensures wrapping between 0–2
        }

        UpdateTextOutlines(); // Update text highlights
    }

    private void UpdateTextOutlines()
    {
        for (int i = 0; i < CurrentMenuTexts.Count; i++)
        {
            if (i == selectionIndex)
            {
                // Start pulsing effect
                StartCoroutine(PulseOutline(CurrentMenuTexts[i]));
                CurrentMenuTexts[i].outlineColor = selectedOutlineColor;
            }
            else
            {
                // Reset to default outline color and width
                CurrentMenuTexts[i].outlineColor = defaultOutlineColor;
                CurrentMenuTexts[i].outlineWidth = 0f;
            }
        }
    }

    private IEnumerator PulseOutline(TextMeshProUGUI text)
    {
        float timer = 0f;

        while (text == CurrentMenuTexts[selectionIndex]) // Pulse only for the current selected text
        {
            timer += Time.deltaTime * pulseSpeed;
            float outlineWidth = Mathf.Lerp(0.1f, maxOutlineWidth, Mathf.PingPong(timer, 1));
            text.outlineWidth = outlineWidth;
            yield return null;
        }
    }

    private IEnumerator MoveCameraAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(cameraMoveDelay);

        if (mainCamera != null && targetTransform != null && !isMovingCamera)
        {
            isMovingCamera = true;
            Vector3 startPosition = mainCamera.transform.position;
            Quaternion startRotation = mainCamera.transform.rotation;
            Vector3 endPosition = targetTransform.position;
            Quaternion endRotation = targetTransform.rotation;

            float elapsedTime = 0f;

            while (elapsedTime < cameraMoveDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / cameraMoveDuration;

                // Smoothly interpolate the camera position and rotation
                mainCamera.transform.position = Vector3.Lerp(startPosition, endPosition, t);
                mainCamera.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);

                yield return null;
            }

            // Ensure camera reaches the exact target position and rotation
            mainCamera.transform.position = endPosition;
            mainCamera.transform.rotation = endRotation;

            isMovingCamera = false;
            BookOpen = true;
        }
    }
}