using UnityEngine;
using System.Collections;

public class AlarmCutsceneManager : MonoBehaviour
{
    [Header("References")]
    public AlarmNode[] alarms;                     // The 7 alarms
    public Camera[] alarmCameras;                  // One camera per alarm
    public PropManager propManager;                // Reference your PropManager
    public AlarmSequenceManager sequenceManager;   // Reference your AlarmSequenceManager
    public MonoBehaviour playerController;         // Player movement script
    public Camera playerCamera;                    // Normal player camera

    [Header("Cutscene Settings")]
    public float timeBetweenAlarms = 0.5f;        // Delay if no audio
    public bool playOnStart = true;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 0f; // 2D sound
        }
    }

    private void Start()
    {
        if (playOnStart)
            PlayCutscene();
    }

    public void PlayCutscene()
    {
        StartCoroutine(CutsceneRoutine());
    }

    private IEnumerator CutsceneRoutine()
    {
        // 1. Freeze gameplay
        if (playerController != null) playerController.enabled = false;
        if (propManager != null) propManager.spawningEnabled = false;
        if (sequenceManager != null) sequenceManager.acceptingInput = false;

        if (playerCamera != null) playerCamera.enabled = false;

        // 2. Loop through each alarm and camera
        for (int i = 0; i < alarms.Length; i++)
        {
            AlarmNode alarm = alarms[i];
            Camera cam = (alarmCameras != null && i < alarmCameras.Length) ? alarmCameras[i] : null;

            if (cam != null) cam.enabled = true;
            if (alarm != null) alarm.RevealCutscene();

            // Play audio if available
            if (alarm != null && alarm.alarmClip != null)
            {
                audioSource.clip = alarm.alarmClip;
                audioSource.Play();
                yield return new WaitForSeconds(alarm.alarmClip.length);
            }
            else
            {
                yield return new WaitForSeconds(timeBetweenAlarms);
            }

            // Reset alarm and camera
            if (alarm != null) alarm.SetIdle();
            if (cam != null) cam.enabled = false;
            // After resuming gameplay
            
            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.enabled = false;
            }
        }

        // 3. Resume gameplay
        if (playerCamera != null) playerCamera.enabled = true;
        if (playerController != null) playerController.enabled = true;
        if (propManager != null) propManager.spawningEnabled = true;
        if (sequenceManager != null); //sequenceManager.StartNewSequence();
    }
}