using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.ProBuilder.MeshOperations;

public class LevelSelect : MonoBehaviour
{
    // Cylinder allocation
    public GameObject[] cylinders;

    // Individual controller selection stuff
    private Dictionary<int, int> controllerSelections = new Dictionary<int, int>();

    // Controller detection
    private readonly Dictionary<Gamepad, int> controllerIDs = new Dictionary<Gamepad, int>();
    private int nextControllerID = 1;

    public bool tavernSelected = true;
    public bool mausoleumSelected = false;

    public GameObject Tavern;
    public GameObject Mausoleum;

    void Start()
    {
        // Assign each detected controller an ID
        foreach (var gamepad in Gamepad.all)
        {
            AssignControllerID(gamepad);
        }

        // Create an outline for each controller
        UpdateAllOutlines();

        Tavern.transform.position = new Vector3(0, 7, 0);
        Mausoleum.transform.position = new Vector3(-36, 0, 213);
    }

    void OnEnable()
    {
        // Check for controller disconnection
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDisable()
    {
        // Check for controller connection?
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void Update()
    {
        // Use joystick for each controller to navigate menu
        foreach (var gamepad in Gamepad.all)
        {
            int controllerID = controllerIDs[gamepad];

            if (gamepad.leftStick.right.wasPressedThisFrame)
            {
                // Move selection to the right
                controllerSelections[controllerID] = (controllerSelections[controllerID] + 1) % cylinders.Length;
                Debug.Log($"Controller {controllerID} moved right, selection: {controllerSelections[controllerID]}");
                UpdateAllOutlines();
                MoveLevel();
            }
            else if (gamepad.leftStick.left.wasPressedThisFrame)
            {
                // Move selection to the left
                controllerSelections[controllerID] = (controllerSelections[controllerID] - 1 + cylinders.Length) % cylinders.Length;
                Debug.Log($"Controller {controllerID} moved left, selection: {controllerSelections[controllerID]}");
                UpdateAllOutlines();
                MoveLevel();
            }

            if (tavernSelected)
            {
                Tavern.transform.Rotate(0, 0.06f, 0);
            }

            if (mausoleumSelected)
            {
                Mausoleum.transform.Rotate(0, 0.06f, 0);
            }

            // Load Tavern level when southern button is pressed
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                if (controllerSelections[controllerID] == 0)
                {
                    Debug.Log($"Controller {controllerID} confirmed selection!");
                    SceneManager.LoadScene(3);
                }
                if (controllerSelections[controllerID] == 1)
                {
                    Debug.Log($"Controller {controllerID} confirmed selection!");
                    SceneManager.LoadScene(4);
                }
            }

            if (controllerSelections[controllerID] == 0)
            {
                tavernSelected = true;
                mausoleumSelected = false;
            }
            if (controllerSelections[controllerID] == 1)
            {
                tavernSelected = false;
                mausoleumSelected = true;
            }
        }

        // Old keyboard input
        if (Input.GetKeyDown(KeyCode.D))
        {
            controllerSelections[1] = (controllerSelections[1] + 1) % cylinders.Length;
            UpdateAllOutlines();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            controllerSelections[1] = (controllerSelections[1] - 1 + cylinders.Length) % cylinders.Length;
            UpdateAllOutlines();
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

        // Move levels based on Player 1's selection
        if (player1Selection == 0)
        {
            tavernSelected = true;
            mausoleumSelected = false;

            // Move Tavern to the front and Mausoleum to the back
            Tavern.transform.position = new Vector3(0, 7, 0);
            Mausoleum.transform.position = new Vector3(-84, 0, 219);  // Behind Tavern
        }
        else if (player1Selection == 1)
        {
            tavernSelected = false;
            mausoleumSelected = true;

            // Move Mausoleum to the front and Tavern to the back
            Mausoleum.transform.position = new Vector3(0, 7, 0);
            Tavern.transform.position = new Vector3(-46, 7, -207);  // Behind Mausoleum
        }

        // Reset rotation for both levels to ensure they face forward correctly
        Tavern.transform.rotation = Quaternion.identity;
        Mausoleum.transform.rotation = Quaternion.identity;
    }

    void UpdateAllOutlines()
    {
        // Select each cylinder
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

                // Disable outline if not selected
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
        // Assign colors based on controller ID
        switch (controllerID)
        {
            case 1:
                return Color.blue; // Player 1
            case 2:
                return Color.red;  // Player 2
            case 3:
                return new Color(1f, 0.41f, 0.71f); // Player 3 - Pink (RGB: 1, 0.41, 0.71)
            case 4:
                return new Color(1f, 0.647f, 0f); // Player 4 - Orange (RGB: 1, 0.647, 0)
            default:
                return Color.gray;
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
            controllerSelections[nextControllerID] = 0; // Puts outline on first cylinder
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
