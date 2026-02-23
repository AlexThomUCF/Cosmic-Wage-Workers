using UnityEngine;
using System.Collections;

public class MultiCameraCutscene : MonoBehaviour
{
    [Header("Cutscene Cameras")]
    public Camera[] cutsceneCameras;
    public float timePerCamera = 3f;

    [Header("Optional Alarm Reveal")]
    public AlarmNode[] alarms; // same order as cameras (or fewer)

    [Header("Systems To Pause")]
    public MonoBehaviour playerController;
    public PropManager propManager;
    public AlarmSequenceManager sequenceManager;

    [Header("Player Camera")]
    public Camera playerCamera;

    [Header("Timing")]
    public float delayBeforeStart = 1f;

    void Start()
    {
        StartCoroutine(CutsceneRoutine());
    }

    IEnumerator CutsceneRoutine()
    {
        yield return new WaitForSeconds(delayBeforeStart);

        // Disable gameplay
        if (playerController) playerController.enabled = false;
        if (propManager) propManager.spawningEnabled = false;

        if (playerCamera) playerCamera.enabled = false;

        foreach (Camera cam in cutsceneCameras)
            cam.enabled = false;

        // Play cameras + alarms
        for (int i = 0; i < cutsceneCameras.Length; i++)
        {
            Camera cam = cutsceneCameras[i];
            cam.enabled = true;

            if (alarms != null && i < alarms.Length && alarms[i] != null)
            {
                alarms[i].RevealCutscene();
            }

            yield return new WaitForSeconds(timePerCamera);

            if (alarms != null && i < alarms.Length && alarms[i] != null)
            {
                alarms[i].SetIdle();
            }

            cam.enabled = false;
        }

        // Restore gameplay
        if (playerCamera) playerCamera.enabled = true;
        if (playerController) playerController.enabled = true;
        if (propManager) propManager.spawningEnabled = true;
    }
}