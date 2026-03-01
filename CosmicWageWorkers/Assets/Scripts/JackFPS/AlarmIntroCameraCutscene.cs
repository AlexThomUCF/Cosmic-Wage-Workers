using UnityEngine;
using System.Collections;

public class MultiCameraCutscene : MonoBehaviour
{
    [Header("Cutscene Cameras")]
    public Camera[] cutsceneCameras;

    [Header("Alarm Order")]
    public AlarmNode[] alarms;

    [Header("Timing")]
    public float delayBeforeStart = 1f;
    public float cameraIntroPause = 0.4f;
    public float alarmHoldTime = 1.6f;
    public float timeBetweenAlarms = 0.3f;

    [Header("Systems To Pause")]
    public MonoBehaviour playerController;
    public MonoBehaviour shootingScript;
    public PropManager propManager;

    [Header("Player Camera")]
    public Camera playerCamera;

    [Header("UI")]
    public GameObject hudCanvas;

    public AlarmSequenceManager sequenceManager;

    void Start()
    {
        StartCoroutine(CutsceneRoutine());
        
    }

    IEnumerator CutsceneRoutine()
    {
        yield return new WaitForSeconds(delayBeforeStart);

        if (playerController) playerController.enabled = false;
        if (propManager) propManager.spawningEnabled = false;
        if (playerCamera) playerCamera.enabled = false;
        if (sequenceManager) sequenceManager.enabled = false;
        if (shootingScript) shootingScript.enabled = false;
        if (hudCanvas) hudCanvas.SetActive(false);
        foreach (Camera cam in cutsceneCameras)
            cam.enabled = false;

        int currentCameraIndex = 0;
        int alarmsPerCamera = Mathf.CeilToInt((float)alarms.Length / cutsceneCameras.Length);

        // Activate first camera
        cutsceneCameras[currentCameraIndex].enabled = true;
        yield return new WaitForSeconds(cameraIntroPause);

        for (int i = 0; i < alarms.Length; i++)
        {
            // Play alarm
            alarms[i].RevealCutscene();
            yield return new WaitForSeconds(alarmHoldTime);
            alarms[i].SetIdle();

            yield return new WaitForSeconds(timeBetweenAlarms);

            // Switch camera if needed
            if ((i + 1) % alarmsPerCamera == 0 &&
                currentCameraIndex < cutsceneCameras.Length - 1)
            {
                cutsceneCameras[currentCameraIndex].enabled = false;
                currentCameraIndex++;
                cutsceneCameras[currentCameraIndex].enabled = true;

                yield return new WaitForSeconds(cameraIntroPause);
            }
        }

        // Restore gameplay
        cutsceneCameras[currentCameraIndex].enabled = false;

        if (playerCamera) playerCamera.enabled = true;
        if (playerController) playerController.enabled = true;
        if (shootingScript) shootingScript.enabled = true;
        if (propManager) propManager.StartSpawning();
        if (sequenceManager) sequenceManager.enabled = false;
        if (hudCanvas) hudCanvas.SetActive(true);
    }
}