using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicPhenomenonManager : MonoBehaviour
{
    [Header("Timing")]
    public float minTimeBetweenEvents = 30f;
    public float maxTimeBetweenEvents = 60f;

    [Header("Dialogue Settings")]
    public float postDialogueDelay = 2f;

    [Header("Solar Flare Settings")]
    public float solarFlareDialogueDelay = 3f;

    [Header("Dialogue Lines")]
    [TextArea] public List<string> solarFlareLines;
    [TextArea] public List<string> antiGravityLines;
    [TextArea] public List<string> blackHoleLines;
    [TextArea] public List<string> eclipseLines;
    [TextArea] public List<string> primordialSoupLines;

    [Header("References")]
    public SolarFlare solarFlare;
    public AntiGravity antiGravity;
    public BlackHoles blackHoles;
    public Eclipse eclipse;
    public PrimordialSoup primordialSoup;

    private void Start()
    {
        StartCoroutine(EventLoop());
    }

    private IEnumerator EventLoop()
    {
        while (true)
        {
            // Wait until dialogue is NOT active
            yield return new WaitUntil(() =>
                DialogueController.Instance == null ||
                !DialogueController.Instance.dialoguePanel.activeSelf
            );

            float waitTime = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
            yield return new WaitForSeconds(waitTime);

            StartCoroutine(HandleEventWithDialogue());
        }
    }

    private IEnumerator HandleEventWithDialogue()
    {
        int eventIndex = Random.Range(0, 5);
        string chosenLine = GetRandomLine(eventIndex);

        // =========================
        // SOLAR FLARE (SPECIAL CASE)
        // =========================
        if (eventIndex == 0)
        {
            // Trigger event FIRST
            TriggerEvent(eventIndex);

            // Wait before dialogue
            yield return new WaitForSeconds(solarFlareDialogueDelay);

            // Show dialogue AFTER
            if (DialogueController.Instance != null)
            {
                DialogueController.Instance.ShowDialogue("Intercom", chosenLine);

                yield return new WaitUntil(() => DialogueController.Instance.IsFinishedTyping());
                yield return new WaitForSeconds(postDialogueDelay);

                DialogueController.Instance.HideDialogue();
            }

            yield break;
        }

        // =========================
        // NORMAL EVENTS
        // =========================
        if (DialogueController.Instance != null)
        {
            DialogueController.Instance.ShowDialogue("Intercom", chosenLine);

            yield return new WaitUntil(() => DialogueController.Instance.IsFinishedTyping());
            yield return new WaitForSeconds(postDialogueDelay);

            DialogueController.Instance.HideDialogue();
        }

        // Trigger AFTER dialogue
        TriggerEvent(eventIndex);
    }

    private string GetRandomLine(int eventIndex)
    {
        List<string> lines = null;

        switch (eventIndex)
        {
            case 0: lines = solarFlareLines; break;
            case 1: lines = antiGravityLines; break;
            case 2: lines = blackHoleLines; break;
            case 3: lines = eclipseLines; break;
            case 4: lines = primordialSoupLines; break;
        }

        if (lines != null && lines.Count > 0)
            return lines[Random.Range(0, lines.Count)];

        return "...";
    }

    private void TriggerEvent(int eventIndex)
    {
        switch (eventIndex)
        {
            case 0:
                if (solarFlare != null) solarFlare.TriggerFlare();
                break;

            case 1:
                if (antiGravity != null) antiGravity.TriggerAntiGravity();
                break;

            case 2:
                if (blackHoles != null) blackHoles.TriggerBlackHoles();
                break;

            case 3:
                if (eclipse != null) eclipse.TriggerEclipse();
                break;

            case 4:
                if (primordialSoup != null && primordialSoup.soupPrefab != null)
                {
                    Transform spawnPoint = primordialSoup.spawnPoints[
                        Random.Range(0, primordialSoup.spawnPoints.Length)
                    ];

                    Instantiate(
                        primordialSoup.soupPrefab,
                        spawnPoint.position,
                        Quaternion.identity
                    );
                }
                break;
        }
    }
}