using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : MonoBehaviour
{
    private readonly Dictionary<Gamepad, int> controllerIDs = new Dictionary<Gamepad, int>();
    private readonly Dictionary<int, GameObject> playerInstances = new Dictionary<int, GameObject>();

    public GameObject player1Object; // Assign this in the Inspector to the first player prefab in the scene
    public GameObject player2Object; // Assign this in the Inspector to the second player prefab in the scene

    private int nextControllerID = 1;

    void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;

        foreach (var gamepad in Gamepad.all)
        {
            AssignControllerID(gamepad);
        }
    }

    void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad gamepad)
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    AssignControllerID(gamepad);
                    break;

                case InputDeviceChange.Removed:
                    RemoveControllerID(gamepad);
                    break;
            }
        }
    }

    private void AssignControllerID(Gamepad gamepad)
    {
        if (!controllerIDs.ContainsKey(gamepad))
        {
            controllerIDs[gamepad] = nextControllerID;

            // Assign the existing player object for the controller
            GameObject playerObject = nextControllerID == 1 ? player1Object : player2Object;

            if (playerObject != null)
            {
                playerInstances[nextControllerID] = playerObject;

                // Assign the controller to the character's movement
                var movementController = playerObject.GetComponent<MovementController>();
                if (movementController != null)
                {
                    movementController.AssignController(gamepad);
                }
                Debug.Log($"Controller {nextControllerID} assigned to {playerObject.name}");
            }
            else
            {
                Debug.LogError($"Player {nextControllerID} object is not assigned in the Inspector!");
            }

            nextControllerID++;
        }
    }


    private void RemoveControllerID(Gamepad gamepad)
    {
        if (controllerIDs.ContainsKey(gamepad))
        {
            int id = controllerIDs[gamepad];

            if (playerInstances.ContainsKey(id))
            {
                Debug.Log($"Controller {id} disconnected from {playerInstances[id].name}");
                playerInstances.Remove(id);
            }

            controllerIDs.Remove(gamepad);
        }
    }
}
