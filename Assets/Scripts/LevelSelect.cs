using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    // Cylinder allocation
    public GameObject[] cylinders;

    // Individual controller selection stuff
    private Dictionary<int, int> controllerSelections = new Dictionary<int, int>();

    // Controller detection
    private readonly Dictionary<Gamepad, int> controllerIDs = new Dictionary<Gamepad, int>();
    private int nextControllerID = 1;

    public bool tavernSelected = false;
    public bool mausoleumSelected = false;
    public bool tutorialSelected = true;

    public GameObject Tavern;
    public GameObject Mausoleum;
    public GameObject Tutorial;

    public TextMeshProUGUI levelNameText;

    void Start()
    {
        // Assign each detected controller an ID
        foreach (var gamepad in Gamepad.all)
        {
            AssignControllerID(gamepad);
        }

        // Create an outline for each controller
        UpdateAllOutlines();
    }

    void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void Update()
    {
        // Restrict input to Player 1 only
        foreach (var gamepad in Gamepad.all)
        {
            if (!controllerIDs.TryGetValue(gamepad, out int controllerID)) continue;

            // Only allow Player 1 (controllerID == 1) to control selection and scene loading
            if (controllerID == 1)
            {
                if (gamepad.leftStick.right.wasPressedThisFrame)
                {
                    controllerSelections[controllerID] = (controllerSelections[controllerID] + 1) % cylinders.Length;
                    Debug.Log($"Player 1 moved right, selection: {controllerSelections[controllerID]}");
                    UpdateAllOutlines();
                    MoveLevel();
                }
                else if (gamepad.leftStick.left.wasPressedThisFrame)
                {
                    controllerSelections[controllerID] = (controllerSelections[controllerID] - 1 + cylinders.Length) % cylinders.Length;
                    Debug.Log($"Player 1 moved left, selection: {controllerSelections[controllerID]}");
                    UpdateAllOutlines();
                    MoveLevel();
                }

                if (gamepad.buttonSouth.wasPressedThisFrame)
                {
                    Debug.Log("Player 1 confirmed selection!");
                    int selection = controllerSelections[controllerID];
                    if (selection == 0) SceneManager.LoadScene(5);
                    else if (selection == 1) SceneManager.LoadScene(3);
                    else if (selection == 2) SceneManager.LoadScene(4);
                }

                if (controllerSelections[controllerID] == 0)
                {
                    tavernSelected = false;
                    mausoleumSelected = false;
                    tutorialSelected = true;
                }
                else if (controllerSelections[controllerID] == 1)
                {
                    tutorialSelected = false;
                    tavernSelected = true;
                    mausoleumSelected = false;
                }
                else if (controllerSelections[controllerID] == 2)
                {
                    tutorialSelected = false;
                    mausoleumSelected = true;
                    tavernSelected = false;
                }
            }
        }

        // Idle animations and text updates (always run)
        if (tavernSelected)
        {
            Tavern.transform.Rotate(0, 0.06f, 0);
            levelNameText.text = "Tavern";
        }

        if (mausoleumSelected)
        {
            Mausoleum.transform.Rotate(0, 0.06f, 0);
            levelNameText.text = "Mausoleum";
        }

        if (tutorialSelected)
        {
            Tutorial.transform.Rotate(0, 0.06f, 0);
            levelNameText.text = "Tutorial";
        }

        // Optional: keyboard input for debugging
        if (Input.GetKeyDown(KeyCode.D))
        {
            controllerSelections[1] = (controllerSelections[1] + 1) % cylinders.Length;
            UpdateAllOutlines();
            MoveLevel();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            controllerSelections[1] = (controllerSelections[1] - 1 + cylinders.Length) % cylinders.Length;
            UpdateAllOutlines();
            MoveLevel();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(3);
        }
    }

    void MoveLevel()
    {
        // Ensure Player 1's selection takes precedence
        int player1Selection = controllerSelections.ContainsKey(1) ? controllerSelections[1] : 0;

        if (player1Selection == 0)
        {
            tavernSelected = false;
            mausoleumSelected = false;
            tutorialSelected = true;

            Tutorial.transform.position = new Vector3(0, 7, 0);
            Tavern.transform.position = new Vector3(-46, 7, 196);
            Mausoleum.transform.position = new Vector3(-131, 7, 375);
        }
        else if (player1Selection == 1)
        {
            tutorialSelected = false;
            tavernSelected = true;
            mausoleumSelected = false;

            Tutorial.transform.position = new Vector3(-125, 7, -153);
            Mausoleum.transform.position = new Vector3(-69, 7, 180);
            Tavern.transform.position = new Vector3(0, 7, 0);
        }
        else if (player1Selection == 2)
        {
            tutorialSelected = false;
            tavernSelected = false;
            mausoleumSelected = true;

            Tutorial.transform.position = new Vector3(-250, 7, -300);
            Mausoleum.transform.position = new Vector3(0, 7, 0);
            Tavern.transform.position = new Vector3(-46, 7, -196);
        }

        // Reset rotation
        Tavern.transform.rotation = Quaternion.identity;
        Mausoleum.transform.rotation = Quaternion.identity;
        Tutorial.transform.rotation = Quaternion.identity;
    }

    void UpdateAllOutlines()
    {
        for (int i = 0; i < cylinders.Length; i++)
        {
            Outline outline = cylinders[i].GetComponent<Outline>();
            if (outline != null)
            {
                bool isSelected = false;
                foreach (var selectedIndex in controllerSelections.Values)
                {
                    if (selectedIndex == i)
                    {
                        isSelected = true;
                        outline.enabled = true;
                        outline.OutlineColor = GetControllerColor(GetControllerIDBySelection(i));
                        break;
                    }
                }

                if (!isSelected)
                {
                    outline.enabled = false;
                }
            }
        }
    }

    int GetControllerIDBySelection(int selectionIndex)
    {
        foreach (var kvp in controllerSelections)
        {
            if (kvp.Value == selectionIndex)
            {
                return kvp.Key;
            }
        }
        return 0;
    }

    Color GetControllerColor(int controllerID)
    {
        switch (controllerID)
        {
            case 1: return Color.blue;
            case 2: return Color.red;
            case 3: return new Color(1f, 0.41f, 0.71f); // Pink
            case 4: return new Color(1f, 0.647f, 0f);   // Orange
            default: return Color.gray;
        }
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad gamepad)
        {
            if (change == InputDeviceChange.Added)
            {
                AssignControllerID(gamepad);
            }
            else if (change == InputDeviceChange.Removed)
            {
                RemoveControllerID(gamepad);
            }
        }
    }

    void AssignControllerID(Gamepad gamepad)
    {
        if (!controllerIDs.ContainsKey(gamepad))
        {
            controllerIDs[gamepad] = nextControllerID;
            controllerSelections[nextControllerID] = 0;
            nextControllerID++;
        }
    }

    void RemoveControllerID(Gamepad gamepad)
    {
        if (controllerIDs.TryGetValue(gamepad, out int controllerID))
        {
            controllerIDs.Remove(gamepad);
            controllerSelections.Remove(controllerID);
        }
    }
}
