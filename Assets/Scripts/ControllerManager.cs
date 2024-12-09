using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : MonoBehaviour
{
    private readonly Dictionary<Gamepad, int> controllerIDs = new Dictionary<Gamepad, int>();
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
            Debug.Log($"Controller {nextControllerID} connected: {gamepad.name}");
            nextControllerID++;
        }
    }

    private void RemoveControllerID(Gamepad gamepad)
    {
        if (controllerIDs.ContainsKey(gamepad))
        {
            int id = controllerIDs[gamepad];
            Debug.Log($"Controller {id} disconnected: {gamepad.name}");
            controllerIDs.Remove(gamepad);
        }
    }

    public int GetControllerID(Gamepad gamepad)
    {
        return controllerIDs.TryGetValue(gamepad, out int id) ? id : -1;
    }

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
