using UnityEngine;
using UnityEngine.InputSystem;

public class DynamicInputScheme : MonoBehaviour
{
    public PlayerInput playerInput;  // Your PlayerInput component

    private bool isGamepadActive;

    void Start()
    {
        SwitchToScheme("Keyboard");
        isGamepadActive = false;
        UpdateScheme(); // Initial check

        // Subscribe to device connect/disconnect
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void Update()
    {
        UpdateScheme(); // Continuously check input each frame
    }

    void UpdateScheme()
    {
        // PRIORITY: If keyboard is used ? switch to keyboard
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            if (isGamepadActive)
            {
                SwitchToScheme("Keyboard");
                isGamepadActive = false;
            }
            return;
        }

        // Only switch to gamepad if a button/stick was actually used
        if (Gamepad.current != null)
        {
            bool gamepadUsed =
                Gamepad.current.buttonSouth.wasPressedThisFrame ||
                Gamepad.current.leftStick.ReadValue().sqrMagnitude > 0.01f;

            if (gamepadUsed && !isGamepadActive)
            {
                SwitchToScheme("Gamepad");
                isGamepadActive = true;
            }
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
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
        {
            // Only pass devices relevant to the scheme
            if (scheme == "Keyboard")
                playerInput.SwitchCurrentControlScheme("Keyboard", Keyboard.current, Mouse.current);
            else if (scheme == "Gamepad")
                playerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.current);

            Debug.Log("Switched to: " + scheme);
        }
    }

    private void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }
}