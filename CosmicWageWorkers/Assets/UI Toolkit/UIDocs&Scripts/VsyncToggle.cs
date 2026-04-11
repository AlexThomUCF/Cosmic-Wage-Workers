using UnityEngine;
using UnityEngine.UI;

public class VsyncToggle : MonoBehaviour
{
    public Toggle vsyncToggle;

    private const string VSyncKey = "VSyncEnabled";

    void Start()
    {
        // Load saved value (default = ON)
        bool isOn = PlayerPrefs.GetInt(VSyncKey, 1) == 1;

        // Apply setting
        ApplyVSync(isOn);

        // Update toggle without triggering event
        vsyncToggle.isOn = isOn;

        // Listen for changes
        vsyncToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        ApplyVSync(isOn);

        // Save preference
        PlayerPrefs.SetInt(VSyncKey, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    void ApplyVSync(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;

        // Optional FPS cap when VSync is OFF
        Application.targetFrameRate = isOn ? -1 : 60;
    }
}