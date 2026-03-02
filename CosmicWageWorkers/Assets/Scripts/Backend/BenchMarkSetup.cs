using UnityEngine;

public class BenchMarkSetup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
        Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);
    }
}
