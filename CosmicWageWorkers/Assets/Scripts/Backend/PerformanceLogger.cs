using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PerformanceLogger : MonoBehaviour
{
    private List<float> frameTimes = new List<float>();
    private float testDuration = 60f;
    private float timer = 0f;

    void Update()
    {
        float frameTime = Time.unscaledDeltaTime * 1000f;
        frameTimes.Add(frameTime);

        timer += Time.unscaledDeltaTime;

        if (timer >= testDuration)
        {
            SaveResults();
            enabled = false;
        }
    }

    void SaveResults()
    {
        float total = 0f;

        foreach (float frameTime in frameTimes)
            total += frameTime;

        float averageFrameTime = total / frameTimes.Count;
        float averageFPS = 1000f / averageFrameTime;

        string path = Application.persistentDataPath + "/benchmark.csv";

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("Average Frame Time (ms): " + averageFrameTime);
            writer.WriteLine("Average FPS: " + averageFPS);
            writer.WriteLine();
            writer.WriteLine("Frame,FrameTime(ms)");

            for (int i = 0; i < frameTimes.Count; i++)
                writer.WriteLine((i + 1) + "," + frameTimes[i]);
        }

        Debug.Log("Average FPS: " + averageFPS);
    }
}
