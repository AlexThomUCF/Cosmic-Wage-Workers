using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class RaceIntroManager : MonoBehaviour
{
    [Header("Cameras")]
    public Camera playerCamera;
    public Camera gameplayStartCamera;
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

    [Header("Countdown SFX")]
    public AudioSource countdownsfx;
    public AudioSource goSFX;

    public GameObject gameplayHUD;

    private Rigidbody[] aiBodies;
    private Coroutine introCoroutine;
    private bool isIntroPlaying = false;
    private bool isSkipping = false;

    void Start()
    {
        Time.timeScale = 1f; // ?? critical fix

        if (playOnStart)
        {
            introCoroutine = StartCoroutine(IntroSequence());
            isIntroPlaying = true;
        }
    }

    void Update()
    {
        if (isIntroPlaying && !isSkipping && Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(SkipToGameplayCountdown());
        }
    }

    IEnumerator IntroSequence()
    {
        // Disable gameplay
        if (playerController != null) playerController.enabled = false;
        if (raceManager != null) raceManager.enabled = false;
        if (gameplayHUD != null) gameplayHUD.SetActive(false);

        if (countdownCanvas != null)
            countdownCanvas.SetActive(false);

        // Freeze AI
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
        DisableAllCameras();

        // Play intro cameras
        for (int i = 0; i < introCameras.Count; i++)
        {
            Camera cam = introCameras[i];
            if (cam == null) continue;

            cam.enabled = true;

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

        EndIntroNormally();
    }

    IEnumerator SkipToGameplayCountdown()
    {
        isSkipping = true;

        if (introCoroutine != null)
            StopCoroutine(introCoroutine);

        // Stop any currently playing countdown sounds just in case
        if (countdownsfx != null) countdownsfx.Stop();
        if (goSFX != null) goSFX.Stop();

        // Disable every other camera
        DisableAllCameras();

        // Enable gameplay start camera for the countdown
        if (gameplayStartCamera != null)
            gameplayStartCamera.enabled = true;

        // Play 3, 2, 1, GO
        yield return StartCoroutine(CountdownRoutine());

        // Disable gameplay start camera when countdown ends
        if (gameplayStartCamera != null)
            gameplayStartCamera.enabled = false;

        EndIntroAfterSkip();
    }

    void DisableAllCameras()
    {
        if (playerCamera != null)
            playerCamera.enabled = false;

        if (gameplayStartCamera != null)
            gameplayStartCamera.enabled = false;

        foreach (Camera cam in introCameras)
        {
            if (cam != null)
                cam.enabled = false;
        }
    }

    void EndIntroNormally()
    {
        DisableAllCameras();

        // Normal path: return to player camera
        if (playerCamera != null)
            playerCamera.enabled = true;

        FinishIntro();
    }

    void EndIntroAfterSkip()
    {
        DisableAllCameras();

        // Skip path: leave gameplayStartCamera off after countdown
        // Turn player camera back on for gameplay
        if (playerCamera != null)
            playerCamera.enabled = true;

        FinishIntro();
    }

    void FinishIntro()
    {
        if (countdownCanvas != null)
            countdownCanvas.SetActive(false);

        // Unfreeze AI
        for (int i = 0; i < aiCars.Length; i++)
        {
            if (aiCars[i] != null)
                aiCars[i].enabled = true;

            if (aiBodies != null && i < aiBodies.Length && aiBodies[i] != null)
                aiBodies[i].isKinematic = false;
        }

        // Resume gameplay
        if (playerController != null) playerController.enabled = true;
        if (raceManager != null) raceManager.enabled = true;
        if (gameplayHUD != null) gameplayHUD.SetActive(true);

        isIntroPlaying = false;
        isSkipping = false;
    }

    IEnumerator CountdownRoutine()
    {
        if (countdownCanvas != null)
            countdownCanvas.SetActive(true);

        if (countdownText != null)
            countdownText.transform.localScale = Vector3.one;

        if (countdownText != null) countdownText.text = "3";
        if (countdownsfx != null) countdownsfx.Play();
        yield return new WaitForSeconds(1f);

        if (countdownText != null) countdownText.text = "2";
        if (countdownsfx != null) countdownsfx.Play();
        yield return new WaitForSeconds(1f);

        if (countdownText != null) countdownText.text = "1";
        if (countdownsfx != null) countdownsfx.Play();
        yield return new WaitForSeconds(1f);

        if (countdownText != null) countdownText.text = "GO!";
        if (goSFX != null) goSFX.Play();
        yield return StartCoroutine(GoPopEffect());

        yield return new WaitForSeconds(0.6f);

        if (countdownCanvas != null)
            countdownCanvas.SetActive(false);
    }

    IEnumerator GoPopEffect()
    {
        if (countdownText == null) yield break;

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