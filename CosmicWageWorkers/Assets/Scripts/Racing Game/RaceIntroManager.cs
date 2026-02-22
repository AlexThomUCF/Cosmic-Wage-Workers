using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaceIntroManager : MonoBehaviour
{
    [Header("Cameras")]
    public Camera playerCamera;
    public List<Camera> introCameras = new List<Camera>();
    public float timePerCamera = 3f;

    [Header("Gameplay Scripts")]
    public KartControllerArcade playerController;
    public RaceManager raceManager;

    [Header("AI Cars")]
    public AiCarController[] aiCars;

    [Header("Start Settings")]
    public bool playOnStart = true;

    private Rigidbody[] aiBodies;

    void Start()
    {
        if (playOnStart)
            StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        // Disable gameplay
        if (playerController != null) playerController.enabled = false;
        if (raceManager != null) raceManager.enabled = false;

        // Freeze AI cars
        aiBodies = new Rigidbody[aiCars.Length];
        for (int i = 0; i < aiCars.Length; i++)
        {
            if (aiCars[i] == null) continue;

            aiCars[i].enabled = false;

            Rigidbody rb = aiCars[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                aiBodies[i] = rb;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }

        // Camera setup
        if (playerCamera != null)
            playerCamera.enabled = false;

        foreach (Camera cam in introCameras)
        {
            if (cam != null)
                cam.enabled = false;
        }

        // Play intro cams
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

        // Unfreeze AI
        for (int i = 0; i < aiCars.Length; i++)
        {
            if (aiCars[i] != null)
                aiCars[i].enabled = true;

            if (aiBodies[i] != null)
                aiBodies[i].isKinematic = false;
        }

        // Resume gameplay
        if (playerController != null) playerController.enabled = true;
        if (raceManager != null) raceManager.enabled = true;
    }
}