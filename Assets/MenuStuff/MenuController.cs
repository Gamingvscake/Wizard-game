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

    //just for moving the camera closer to the book after it has opened
    [Tooltip("Camera Movement Settings")]
    public Camera mainCamera;
    public Transform targetTransform;
    public float cameraMoveDelay = 1.0f;
    public float cameraMoveDuration = 2.0f;


    //lists for each tmptext on each "page", added to in inspector
    [Tooltip("Selection Settings")]
    public List<TextMeshProUGUI> MainMenuTexts;
    public List<TextMeshProUGUI> SettingsMenuTexts;
    public List<TextMeshProUGUI> GeneralMenuTexts;
    public List<TextMeshProUGUI> AudioMenuTexts;
    public List<TextMeshProUGUI> ControlsMenuTexts;
    public List<TextMeshProUGUI> CurrentMenuTexts;
    public Color defaultOutlineColor = new Color(0, 0, 0, 0); // no outline by default
    public Color selectedOutlineColor = new Color(215, 215, 215); // whiteish outline for selected text
    public float pulseSpeed = 2.0f; // speed of the pulsing effect
    public float maxOutlineWidth = 0.3f; // maximum outline width during the pulse
    private int selectionIndex = 0; // current selected index

    //each canvas that serves as a "page"
    [Tooltip("Screen Objects")]
    public GameObject MenuScreen;
    public GameObject SettingsScreen;
    public GameObject GeneralSettings;
    public GameObject AudioSettings;
    public GameObject ControlsSettings;

    //booleans for what's happening (which page, book open, camera moving, etc)
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

        //sets current "open page" to the menu, sets every other page to false
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
        //every menu selection option, decided based on the "text selected" (the selectionIndex value)
        if (BookOpen && Input.GetButtonDown("Jump"))
        {
            if (MainScreenOpen)
            {
                //Start game
                if (selectionIndex == 0)
                {
                    SceneManager.LoadScene(2);
                }
                //Go to settings
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
                //Quit game
                if (selectionIndex == 2)
                {
                    //line below is only for actually running the game
                    Application.Quit();
                    //line below is only for play mode (testing quit variant)
                    UnityEditor.EditorApplication.isPlaying = false;
                }
            }
            else if (SettingsScreenOpen)
            {
                //General settings
                if (selectionIndex == 0)
                {
                    GeneralSettingsOpen = true;
                    CurrentMenuTexts = GeneralMenuTexts;
                    UpdateTextOutlines();
                    GeneralSettings.SetActive(true);
                    SettingsScreen.SetActive(false);
                    SettingsScreenOpen = false;
                }
                //Audio open
                if (selectionIndex == 1)
                {
                    AudioSettingsOpen = true;
                    CurrentMenuTexts = AudioMenuTexts;
                    ChangeSelection(-1);
                    UpdateTextOutlines();
                    AudioSettings.SetActive(true);
                    SettingsScreen.SetActive(false);
                    SettingsScreenOpen = false;
                }
                //Controls settings
                if (selectionIndex == 2)
                {
                    ControlsSettingsOpen = true;
                    CurrentMenuTexts = ControlsMenuTexts;
                    ChangeSelection(2);
                    UpdateTextOutlines();
                    ControlsSettings.SetActive(true);
                    SettingsScreen.SetActive(false);
                    SettingsScreenOpen = false;
                }
                //Go back to main menu
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
            if (GeneralSettingsOpen)
            {
                if (selectionIndex == 3)
                {
                    SettingsScreenOpen = true;
                    CurrentMenuTexts = SettingsMenuTexts;
                    ChangeSelection(1);
                    UpdateTextOutlines();
                    SettingsScreen.SetActive(true);
                    GeneralSettings.SetActive(false);
                    GeneralSettingsOpen = false;
                }
            }
            if (AudioSettingsOpen)
            {
                //Go back
                if (selectionIndex == 3)
                {
                    SettingsScreenOpen = true;
                    CurrentMenuTexts = SettingsMenuTexts;
                    ChangeSelection(1);
                    UpdateTextOutlines();
                    SettingsScreen.SetActive(true);
                    AudioSettings.SetActive(false);
                    AudioSettingsOpen = false;
                }
            }
            if (ControlsSettingsOpen)
            {
                if (selectionIndex == 3)
                {
                    SettingsScreenOpen = true;
                    CurrentMenuTexts = SettingsMenuTexts;
                    ChangeSelection(1);
                    UpdateTextOutlines();
                    SettingsScreen.SetActive(true);
                    ControlsSettings.SetActive(false);
                    ControlsSettingsOpen = false;
                }
            }

        }
        //alternative "back" method using the right button (B on xbox)
        if (BookOpen && Input.GetButtonDown("Crouch"))
        {
            if (ControlsSettingsOpen || AudioSettingsOpen || GeneralSettingsOpen)
            {   
                SettingsScreen.SetActive(true);
                SettingsScreenOpen = true;
                GeneralSettingsOpen = false;
                AudioSettingsOpen = false;
                ControlsSettingsOpen = false;
                GeneralSettings.SetActive(false);
                AudioSettings.SetActive(false);
                ControlsSettings.SetActive(false);
                CurrentMenuTexts = SettingsMenuTexts;
                selectionIndex = 0;
                UpdateTextOutlines();

            }
            else if (SettingsScreenOpen)
            {
                MenuScreen.SetActive(true);
                MainScreenOpen = true;
                CurrentMenuTexts = MainMenuTexts;
                selectionIndex = 0;
                UpdateTextOutlines();
                SettingsScreen.SetActive(false);
                SettingsScreenOpen = false;
            }
            {
                
            }
        }
    }

    private void ChangeSelection(int direction)
    {
        //update selection index and wrap around

        if (SettingsScreenOpen || GeneralSettingsOpen || AudioSettingsOpen || ControlsSettingsOpen)
        {
            selectionIndex = (selectionIndex + direction + 4) % 4; //wraps selection number around 0-3
        }
        else
        {
            selectionIndex = (selectionIndex + direction + 3) % 3; // Modulo ensures wrapping between 0–2
        }

        UpdateTextOutlines(); //update text highlights
    }

    private void UpdateTextOutlines()
    {
        for (int i = 0; i < CurrentMenuTexts.Count; i++)
        {
            if (i == selectionIndex)
            {
                //start pulsing effect
                StartCoroutine(PulseOutline(CurrentMenuTexts[i]));
                CurrentMenuTexts[i].outlineColor = selectedOutlineColor;
            }
            else
            {
                //reset to default outline color and width
                CurrentMenuTexts[i].outlineColor = defaultOutlineColor;
                CurrentMenuTexts[i].outlineWidth = 0f;
            }
        }
    }

    private IEnumerator PulseOutline(TextMeshProUGUI text)
    {
        float timer = 0f;

        while (text == CurrentMenuTexts[selectionIndex]) //pulse only for the current selected text
        {
            timer += Time.deltaTime * pulseSpeed;
            float outlineWidth = Mathf.Lerp(0.1f, maxOutlineWidth, Mathf.PingPong(timer, 1));
            text.outlineWidth = outlineWidth;
            yield return null;
        }
    }

    private IEnumerator MoveCameraAfterDelay()
    {
        //wait for the specified delay
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

                //smoothly move the camera position and rotation
                mainCamera.transform.position = Vector3.Lerp(startPosition, endPosition, t);
                mainCamera.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);

                yield return null;
            }

            //ensure the camera reaches the exact target position and rotation
            mainCamera.transform.position = endPosition;
            mainCamera.transform.rotation = endRotation;

            isMovingCamera = false;
            BookOpen = true;
        }
    }
}