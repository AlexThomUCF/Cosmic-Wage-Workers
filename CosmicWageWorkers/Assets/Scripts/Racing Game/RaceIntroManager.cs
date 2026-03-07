using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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

    [Header("Countdown UI")]
    public GameObject countdownCanvas;
    public TextMeshProUGUI countdownText;

    public GameObject gameplayHUD;

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
        if (gameplayHUD != null) gameplayHUD.SetActive(false);

        if (countdownCanvas != null)
            countdownCanvas.SetActive(false);

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

        // Play intro cameras
        for (int i = 0; i < introCameras.Count; i++)
        {
            Camera cam = introCameras[i];
            if (cam == null) continue;

            cam.enabled = true;

            // If this is the LAST camera → run countdown
            if (i == introCameras.Count - 1)
            {
                yield return StartCoroutine(CountdownRoutine());
            }
            else
            {
                yield return new WaitForSeconds(timePerCamera);
            }

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

        // Resume gameplay AFTER countdown
        if (playerController != null) playerController.enabled = true;
        if (raceManager != null) raceManager.enabled = true;
        if (gameplayHUD != null) gameplayHUD.SetActive(true);
    }

        IEnumerator CountdownRoutine()
    {
        if (countdownCanvas != null)
            countdownCanvas.SetActive(true);

        countdownText.transform.localScale = Vector3.one;

        countdownText.text = "3";
        yield return new WaitForSeconds(1f);

        countdownText.text = "2";
        yield return new WaitForSeconds(1f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        // GO POP EFFECT
        countdownText.text = "GO!";
        yield return StartCoroutine(GoPopEffect());

        yield return new WaitForSeconds(0.6f);

        if (countdownCanvas != null)
            countdownCanvas.SetActive(false);
    }

        IEnumerator GoPopEffect()
    {
        float duration = 0.25f;
        float timer = 0f;

        Vector3 startScale = Vector3.one * 0.5f;
        Vector3 endScale = Vector3.one * 1.5f;

        countdownText.transform.localScale = startScale;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            countdownText.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        countdownText.transform.localScale = Vector3.one;
    }
}