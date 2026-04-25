using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class DynamicInputScheme : MonoBehaviour
{
    public PlayerInput playerInput;

    private bool isGamepadActive;

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
        StartCoroutine(InitializeNextFrame());
    }

    private IEnumerator InitializeNextFrame()
    {
        // Wait one frame so PlayerInput finishes setup
        yield return null;

        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        SwitchToScheme("Keyboard");
        isGamepadActive = false;
    }

    void Update()
    {
        UpdateScheme();
    }

    void UpdateScheme()
    {
        // Keyboard takes priority
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            if (isGamepadActive)
            {
                SwitchToScheme("Keyboard");
                isGamepadActive = false;
            }
            return;
        }

        // Detect real gamepad input
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
        if (device is Gamepad &&
            (change == InputDeviceChange.Added || change == InputDeviceChange.Removed))
        {
            UpdateScheme();
        }
    }

    private void SwitchToScheme(string scheme)
    {
        if (playerInput == null)
            return;

        // EXTRA SAFETY: make sure PlayerInput is actually initialized
        if (!playerInput.isActiveAndEnabled)
            return;

        try
        {
            if (scheme == "Keyboard" && Keyboard.current != null)
            {
                playerInput.SwitchCurrentControlScheme("Keyboard", Keyboard.current, Mouse.current);
            }
            else if (scheme == "Gamepad" && Gamepad.current != null)
            {
                playerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.current);
            }

            Debug.Log("Switched to: " + scheme);
        }
        catch
        {
            // Prevent crash spam if Unity is still initializing
        }
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }
}