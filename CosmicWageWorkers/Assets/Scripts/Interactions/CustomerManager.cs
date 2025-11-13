using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class CustomerManager : MonoBehaviour
{
    [Header("Interactions")]
    public List<CustomerInteraction> allInteractions;
    private List<CustomerInteraction> availableInteractions = new();

    [Header("Spawn Settings")]
    public List<Transform> spawnPoints; // Assign your 5 aisles in order

    [Header("Cinemachine Cameras")]
    public CinemachineCamera mainCam;
    public CinemachineCamera intercomCam;

    [Header("UI")]
    public TextMeshProUGUI dialogueText;

    [Header("Audio")]
    public AudioSource intercomAudio;
    [Tooltip("Assign 5 clips for aisle 1–5 in order")]
    public AudioClip[] aisleAnnouncementClips;

    [Header("Settings")]
    public float minTime = 10f;
    public float maxTime = 10f;

    private bool interactionActive = false;
    private static HashSet<string> completedInteractions = new();

    void Start()
    {
        // Build list of interactions that haven’t been done yet
        availableInteractions = new List<CustomerInteraction>(
            allInteractions.FindAll(i => !completedInteractions.Contains(i.interactionID))
        );

        StartCoroutine(RandomCustomerRoutine());
    }

    IEnumerator RandomCustomerRoutine()
    {
        while (true)
        {
            if (!interactionActive && availableInteractions.Count > 0)
            {
                float waitTime = Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(waitTime);

                yield return StartCoroutine(HandleCustomerInteraction());
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator HandleCustomerInteraction()
    {
        interactionActive = true;

        // Switch to intercom camera
        mainCam.Priority = 0;
        intercomCam.Priority = 10;
        yield return new WaitForSeconds(1.5f);

        // Pick random interaction
        int index = Random.Range(0, availableInteractions.Count);
        CustomerInteraction chosen = availableInteractions[index];

        // Pick random spawn point (aisle)
        int aisleIndex = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[aisleIndex];

        // Spawn the customer
        Instantiate(chosen.customerPrefab, spawnPoint.position, spawnPoint.rotation);

        // Show on-screen dialogue
        dialogueText.text = $"A customer has appeared at {spawnPoint.name}!";

        // Play intercom announcement for that aisle
        if (intercomAudio != null && aisleAnnouncementClips.Length > aisleIndex)
            intercomAudio.PlayOneShot(aisleAnnouncementClips[aisleIndex]);

        yield return new WaitForSeconds(3f);

        dialogueText.text = "";
        intercomCam.Priority = 0;
        mainCam.Priority = 10;
    }

    public static void MarkInteractionComplete(string interactionID)
    {
        completedInteractions.Add(interactionID);
    }

    public void OnReturnToMainScene()
    {
        interactionActive = false;

        availableInteractions = new List<CustomerInteraction>(
            allInteractions.FindAll(i => !completedInteractions.Contains(i.interactionID))
        );

        StartCoroutine(RandomCustomerRoutine());
    }
}