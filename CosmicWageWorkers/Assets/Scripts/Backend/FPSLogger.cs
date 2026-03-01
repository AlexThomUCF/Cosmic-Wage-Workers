using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;  // Needed for accessing the scene name

public class FPSLogger : MonoBehaviour
{
    private float elapsedTime = 0f;
    private int frameCount = 0;
    private StreamWriter sw;

    void Start()
    {
        // Add scene name to the file path to avoid overwriting
        string sceneName = SceneManager.GetActiveScene().name;  // Get the current scene name
        string path = Application.persistentDataPath + "/" + sceneName + "_mimmaxavg.csv";  // Unique file name based on scene

        // Debug log to verify the file path
        Debug.Log("Saving file to: " + path);

        sw = new StreamWriter(path, true);
        sw.WriteLine("Frame, Min FPS, Max FPS, Avg FPS");
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        frameCount++;

        if (elapsedTime >= 1f) // every second
        {
            float fps = frameCount / elapsedTime;
            Debug.Log($"FPS: {fps}");  // Confirm FPS calculation
            sw.WriteLine($"{Time.frameCount}, {fps}");
            frameCount = 0;
            elapsedTime = 0f;
        }
    }

    void OnApplicationQuit()
    {
        sw.Flush();  // Make sure data is written to the file
        sw.Close();  // Close the StreamWriter
    }
}