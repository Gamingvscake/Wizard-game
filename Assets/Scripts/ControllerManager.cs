using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : MonoBehaviour
{
    //Make a dictionary to assign gamepads IDs
    private readonly Dictionary<Gamepad, int> controllerIDs = new Dictionary<Gamepad, int>();
    private int nextControllerID = 1;

    void OnEnable()
    {
        //When it detects a new controller it assigns that controller an ID, this makes it work mid game too
        InputSystem.onDeviceChange += OnDeviceChange;

        foreach (var gamepad in Gamepad.all)
        {
            AssignControllerID(gamepad);
        }
    }

    //This make a controller DC possible
    void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    //This is just the logic for detected new and DCed controllers
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

    //This detects any gamepads and assigns them an ID to be reference to
    private void AssignControllerID(Gamepad gamepad)
    {
        if (!controllerIDs.ContainsKey(gamepad))
        {
            controllerIDs[gamepad] = nextControllerID;
            Debug.Log($"Controller {nextControllerID} connected: {gamepad.name}");
            nextControllerID++;
        }
    }

    //I mean just read the void name its kinda obvious
    private void RemoveControllerID(Gamepad gamepad)
    {
        if (controllerIDs.ContainsKey(gamepad))
        {
            int id = controllerIDs[gamepad];
            Debug.Log($"Controller {id} disconnected: {gamepad.name}");
            controllerIDs.Remove(gamepad);
        }
    }

    //This shows all gamepad IDs in a public integer
    public int GetControllerID(Gamepad gamepad)
    {
        return controllerIDs.TryGetValue(gamepad, out int id) ? id : -1;
    }

    //AAAAAAAAAAAAAAA
    public Gamepad GetControllerByID(int id)
    {
        foreach (var kvp in controllerIDs)
        {
            if (kvp.Value == id)
                return kvp.Key;
        }
        return null;
    }
}
