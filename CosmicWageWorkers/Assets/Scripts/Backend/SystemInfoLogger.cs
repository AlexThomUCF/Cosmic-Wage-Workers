using UnityEngine;

public class SystemInfoLogger : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== System Specs ===");
        Debug.Log("Device Name: " + SystemInfo.deviceName);
        Debug.Log("Device Model: " + SystemInfo.deviceModel);
        Debug.Log("Graphics Device: " + SystemInfo.graphicsDeviceName);
        Debug.Log("Graphics API: " + SystemInfo.graphicsDeviceType);
        Debug.Log("Graphics Memory (MB): " + SystemInfo.graphicsMemorySize);
        Debug.Log("CPU: " + SystemInfo.processorType + " (" + SystemInfo.processorCount + " cores)");
        Debug.Log("System Memory (MB): " + SystemInfo.systemMemorySize);
        Debug.Log("Operating System: " + SystemInfo.operatingSystem);
        Debug.Log("Max Texture Size: " + SystemInfo.maxTextureSize);
        Debug.Log("====================");
    }
}