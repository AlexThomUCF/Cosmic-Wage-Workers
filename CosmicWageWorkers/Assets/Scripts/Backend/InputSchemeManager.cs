using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class InputSchemeManager : MonoBehaviour
{
    public PlayerInput playerInput;                  // Your PlayerInput component
    //public InputSystemUIInputModule uiInputModule;  // Your UI Input Module

    void Start()
    {
        // If a gamepad is connected, switch to Gamepad
        if (Gamepad.current != null)
        {
            SetScheme("Gamepad");
        }
        else
        {
            SetScheme("Keyboard");
        }

        // Subscribe to gamepad connection events
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad)
        {
            if (change == InputDeviceChange.Added)
            {
                SetScheme("Gamepad");
            }
            else if (change == InputDeviceChange.Removed)
            {
                SetScheme("Keyboard");
            }
        }
    }

    private void SetScheme(string scheme)
    {
        if (playerInput != null)
            playerInput.defaultControlScheme = scheme;
    }

    private void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }
}