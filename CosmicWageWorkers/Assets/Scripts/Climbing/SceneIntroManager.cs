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
    public MonoBehaviour playerController;   // your controller
    public MonoBehaviour gameManager;        // your Manager
    public GameObject gameplayHUD;           // UI canvas

    [Header("Start Settings")]
    public bool playOnStart = true;

    void Start()
    {
        if (playOnStart)
            StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        // Disable gameplay
        if (playerController != null) playerController.enabled = false;
        if (gameManager != null) gameManager.enabled = false;
        if (gameplayHUD != null) gameplayHUD.SetActive(false);

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

        // Restore player camera
        if (playerCamera != null)
            playerCamera.enabled = true;

        // Re-enable gameplay
        if (playerController != null) playerController.enabled = true;
        if (gameManager != null) gameManager.enabled = true;
        if (gameplayHUD != null) gameplayHUD.SetActive(true);
    }
}