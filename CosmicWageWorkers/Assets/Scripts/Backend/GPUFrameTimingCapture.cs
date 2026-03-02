using UnityEngine;
using UnityEngine.Rendering;

public class GPUFrameTimingCapture : MonoBehaviour
{
    private FrameTiming[] frameTimings = new FrameTiming[1];

    void Update()
    {
        // Capture frame timings (returns void)
        FrameTimingManager.CaptureFrameTimings();

        // Get latest timings (returns uint ? cast to int)
        int count = (int)FrameTimingManager.GetLatestTimings(1, frameTimings);

        if (count > 0)
        {
            // cpuFrameTime and gpuFrameTime are doubles ? cast to float
            float cpuFrameTimeMs = (float)frameTimings[0].cpuFrameTime;
            float gpuFrameTimeMs = (float)frameTimings[0].gpuFrameTime;

            Debug.Log($"CPU Frame Time: {cpuFrameTimeMs} ms, GPU Frame Time: {gpuFrameTimeMs} ms");
        }
    }
}