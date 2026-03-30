using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class DynamicInputScheme : MonoBehaviour
{
    public PlayerInput playerInput;                  // Your PlayerInput component
    //public InputSystemUIInputModule uiInputModule;  // Your UI Input Module

    private bool isGamepadActive;

    void Start()
    {
        UpdateScheme(); // Initial check

        // Subscribe to device connect/disconnect
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void UpdateScheme()
    {
        // If any gamepad is connected, switch to Gamepad, else Keyboard&Mouse
        bool gamepadConnected = Gamepad.current != null;

        if (gamepadConnected && !isGamepadActive)
        {
            SwitchToScheme("Gamepad");
            isGamepadActive = true;
        }
        else if (!gamepadConnected && isGamepadActive)
        {
            SwitchToScheme("Keyboard");
            isGamepadActive = false;
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        // Only respond to gamepads being added or removed
        if (device is Gamepad)
        {
            if (change == InputDeviceChange.Added || change == InputDeviceChange.Removed)
            {
                UpdateScheme();
            }
        }
    }

    private void SwitchToScheme(string scheme)
    {
        if (playerInput != null)
            playerInput.SwitchCurrentControlScheme(scheme, Keyboard.current, Mouse.current, Gamepad.current);

        /*if (uiInputModule != null)
            uiInputModule.defaultActionAsset.Enable(); // ensure UI actions are active*/

        Debug.Log("Switched to: " + scheme);
    }

    private void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }
}