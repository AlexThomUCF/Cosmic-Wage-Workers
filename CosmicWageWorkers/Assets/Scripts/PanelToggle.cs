using UnityEngine;
using UnityEngine.InputSystem;

public class PanelToggle : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private bool isOpen = false;

    void Start()
    {
        ClosePanel(); // start closed
    }

    void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame) // you can change key
        {
            TogglePanel();
        }
    }

    public void TogglePanel()
    {
        if (isOpen)
            ClosePanel();
        else
            OpenPanel();
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
        isOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f; // pause game (optional)
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        isOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f; // resume game (optional)
    }
}