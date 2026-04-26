using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneIntroManager : MonoBehaviour
{
    [Header("Cameras")]
    public Camera playerCamera;
    public List<Camera> introCameras = new List<Camera>();
    public float timePerCamera = 3f;

    [Header("Gameplay To Disable")]
    public MonoBehaviour playerController;
    public MonoBehaviour gameManager;
    public MonoBehaviour climbing;
    //public MonoBehaviour pausePlayerController;
    public GameObject gameplayHUD;

    [Header("Start Settings")]
    public bool playOnStart = true;

    // NEW
    private Coroutine introCoroutine;
    private bool isIntroPlaying = false;

    void Start()
    {
        if (playOnStart)
        {
            introCoroutine = StartCoroutine(IntroSequence());
            isIntroPlaying = true;
        }
    }

    void Update()
    {
        // Press Space to skip
        if (isIntroPlaying && Input.GetKeyDown(KeyCode.T))
        {
            SkipIntro();
        }
    }

    IEnumerator IntroSequence()
    {
        // Disable gameplay
        if (playerController != null) playerController.enabled = false;
        if (gameManager != null) gameManager.enabled = false;
        if (gameplayHUD != null) gameplayHUD.SetActive(false);
        if (climbing != null) climbing.enabled = false;
        //if (pausePlayerController != null) pausePlayerController.enabled = false;

        // Camera setup
        if (playerCamera != null)
            playerCamera.enabled = false;

        foreach (Camera cam in introCameras)
        {
            if (cam != null)
                cam.enabled = false;
        }

        // Play intro cameras
        foreach (Camera cam in introCameras)
        {
            if (cam == null) continue;

            cam.enabled = true;
            yield return new WaitForSeconds(timePerCamera);
            cam.enabled = false;
        }

        EndIntro();
    }

    void SkipIntro()
    {
        if (introCoroutine != null)
            StopCoroutine(introCoroutine);

        EndIntro();
    }

    void EndIntro()
    {
        // Turn off all intro cameras
        foreach (Camera cam in introCameras)
        {
            if (cam != null)
                cam.enabled = false;
        }

        // Restore player camera
        if (playerCamera != null)
            playerCamera.enabled = true;

        // Re-enable gameplay
        if (playerController != null) playerController.enabled = true;
        if (gameManager != null) gameManager.enabled = true;
        if (gameplayHUD != null) gameplayHUD.SetActive(true);
        if (climbing != null) climbing.enabled = true;
        //if (pausePlayerController != null) pausePlayerController.enabled = true;

        isIntroPlaying = false;
    }
}