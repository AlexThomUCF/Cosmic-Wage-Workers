using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class FPSLogger : MonoBehaviour
{
    private float elapsedTime = 0f;
    private int frameCount = 0;
    private StreamWriter sw;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (sw != null)
        {
            sw.Flush();
            sw.Close();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Close old file if exists
        if (sw != null)
        {
            sw.Flush();
            sw.Close();
        }

        string path = Application.persistentDataPath + "/" + scene.name + "_mimmaxavg.csv";

        Debug.Log("Saving file to: " + path);

        sw = new StreamWriter(path, true);
        sw.WriteLine("Frame, FPS");
    }

    void Update()
    {
        if (sw == null) return;

        elapsedTime += Time.deltaTime;
        frameCount++;

        if (elapsedTime >= 1f)
        {
            float fps = frameCount / elapsedTime;
            sw.WriteLine($"{Time.frameCount}, {fps}");
            frameCount = 0;
            elapsedTime = 0f;
        }
    }
}