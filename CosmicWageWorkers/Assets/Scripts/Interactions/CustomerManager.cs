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
    public List<Transform> spawnPoints;

    [Header("Cinemachine Cameras")]
    public CinemachineCamera mainCam;
    public CinemachineCamera intercomCam;

    [Header("UI")]
    public TextMeshProUGUI dialogueText;

    [Header("Audio")]
    public AudioSource intercomAudio;
    public AudioClip[] aisleAnnouncementClips;
    public AudioSource Megaphone;

    [Header("Player References")]
    public BoxPickUp playerBoxPickup;
    public PickupMop playerMopPickup;

    [Header("Settings")]
    public float minTime = 10f;
    public float maxTime = 10f;

    private bool interactionActive = false;
    private static HashSet<string> completedInteractions = new();
    private CosmicPhenomenonManager cosmicManager;

    void Start()
    {
        SaveSystem.LoadGame();

        availableInteractions = new List<CustomerInteraction>(
            allInteractions.FindAll(i => !completedInteractions.Contains(i.interactionID))
        );

        cosmicManager = FindAnyObjectByType<CosmicPhenomenonManager>();

        StartCoroutine(RandomCustomerRoutine());
    }

    IEnumerator RandomCustomerRoutine()
    {
        while (true)
        {
            if (!interactionActive && availableInteractions.Count > 0)
            {
                float wait = Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(wait);

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

        if (playerBoxPickup != null)
            playerBoxPickup.ForceDropBox();

        if (playerMopPickup != null)
            playerMopPickup.ForceDropMop();

        // Switch to intercom cam and pause phenomena
        mainCam.Priority = 0;
        intercomCam.Priority = 10;
        if (cosmicManager != null)
            cosmicManager.isPaused = true;

        yield return new WaitForSeconds(1f);

        Megaphone.Play();

        yield return new WaitForSeconds(1.5f);

        int index = Random.Range(0, availableInteractions.Count);
        CustomerInteraction chosen = availableInteractions[index];

        int aisleIndex = Random.Range(0, spawnPoints.Count);
        Transform spawn = spawnPoints[aisleIndex];

        Instantiate(chosen.customerPrefab, spawn.position, spawn.rotation);

        dialogueText.text = $"A customer has appeared at {spawn.name}!";

        if (intercomAudio != null && aisleAnnouncementClips.Length > aisleIndex)
            intercomAudio.PlayOneShot(aisleAnnouncementClips[aisleIndex]);

        yield return new WaitForSeconds(3f);

        dialogueText.text = "";

        // Switch back to main cam and resume phenomena
        intercomCam.Priority = 0;
        mainCam.Priority = 10;
        if (cosmicManager != null)
            cosmicManager.isPaused = false;

        interactionActive = false;
    }

    public static void MarkInteractionComplete(string id)
    {
        completedInteractions.Add(id);
    }

    public void OnReturnToMainScene()
    {
        interactionActive = false;

        availableInteractions = new List<CustomerInteraction>(
            allInteractions.FindAll(i => !completedInteractions.Contains(i.interactionID))
        );

        StartCoroutine(RandomCustomerRoutine());
    }

    public static HashSet<string> GetCompletedInteractions()
    {
        return completedInteractions;
    }

    public static void SetCompletedInteractions(List<string> ids)
    {
        completedInteractions = new HashSet<string>(ids);
    }

    private void OnApplicationQuit()
    {
        SaveSystem.SaveGame();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveSystem.SaveGame();
    }
}