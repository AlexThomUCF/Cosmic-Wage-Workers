using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Header("Interactions")]
    public List<CustomerInteraction> allInteractions;
    [SerializeField] private List<CustomerInteraction> availableInteractions = new();

    [Header("Spawn Settings")]
    public List<Transform> spawnPoints;

    [Header("Cinemachine Cameras")]
    public CinemachineCamera mainCam;
    public CinemachineCamera intercomCam;

    [Header("UI")]
    public GameObject intercomPanel;
    public TextMeshProUGUI dialogueText;

    [Header("Audio")]
    public AudioSource intercomAudio;
    public AudioClip[] aisleAnnouncementClips;
    public AudioClip Megaphone;

    [Header("Player References")]
    public BoxPickUp playerBoxPickup;
    public PickupMop playerMopPickup;
    public SqueegeePickup playerSqueegeePickup;

    [Header("Settings")]
    public float minTime = 10f;
    public float maxTime = 10f;

    [Header("Task Spawn Logic")]
    public int tasksUntilGuaranteedSpawn = 5;
    public float spawnChancePerTask = 0.2f;

    private int tasksCompleted = 0;
    private bool customerSpawnedThisScene = false;
    private bool interactionActive = false;

    private static HashSet<string> completedInteractions = new();
    private CosmicPhenomenonManager cosmicManager;

    private Coroutine customerRoutine;

    void Start()
    {
        SaveSystem.LoadGame();

        cosmicManager = FindAnyObjectByType<CosmicPhenomenonManager>();

        InitializeScene();
    }

    void InitializeScene()
    {
        interactionActive = false;
        customerSpawnedThisScene = false;

        mainCam.Priority = 10;
        intercomCam.Priority = 0;

        availableInteractions = new List<CustomerInteraction>(
            allInteractions.FindAll(i => !completedInteractions.Contains(i.interactionID))
        );

        if (customerRoutine != null)
            StopCoroutine(customerRoutine);

        customerRoutine = StartCoroutine(RandomCustomerRoutine());
    }

    IEnumerator RandomCustomerRoutine()
    {
        while (true)
        {
            if (!interactionActive && !customerSpawnedThisScene && availableInteractions.Count > 0)
            {
                float wait = Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(wait);

                if (interactionActive || customerSpawnedThisScene)
                    continue;

                yield return StartCoroutine(HandleCustomerInteraction());
                customerSpawnedThisScene = true;
            }
            else
            {
                yield return null;
            }
        }
    }

    public void OnTaskCompleted()
    {
        if (interactionActive || customerSpawnedThisScene)
            return;

        tasksCompleted++;

        if (tasksCompleted >= tasksUntilGuaranteedSpawn)
        {
            StartCoroutine(HandleCustomerInteraction());
            customerSpawnedThisScene = true;
            return;
        }

        if (Random.value <= spawnChancePerTask)
        {
            StartCoroutine(HandleCustomerInteraction());
            customerSpawnedThisScene = true;
        }
    }

    IEnumerator HandleCustomerInteraction()
    {
        interactionActive = true;

        if (playerBoxPickup != null)
            playerBoxPickup.ForceDropBox();

        if (playerMopPickup != null)
            playerMopPickup.ForceDropMop();

        if (playerSqueegeePickup != null)
            playerSqueegeePickup.DropSqueegee();

        mainCam.Priority = 0;
        intercomCam.Priority = 10;

        if (cosmicManager != null)
            cosmicManager.isPaused = true;

        yield return new WaitForSeconds(1f);

        if (intercomAudio != null && Megaphone != null)
            intercomAudio.PlayOneShot(Megaphone);

        yield return new WaitForSeconds(1.5f);

        int index = Random.Range(0, availableInteractions.Count);
        CustomerInteraction chosen = availableInteractions[index];

        int aisleIndex = Random.Range(0, spawnPoints.Count);
        Transform spawn = spawnPoints[aisleIndex];

        Instantiate(chosen.customerPrefab, spawn.position, spawn.rotation);

        if (intercomPanel != null)
            intercomPanel.SetActive(true);

        dialogueText.text = $"A customer has appeared at {spawn.name}!";

        if (intercomAudio != null && aisleAnnouncementClips.Length > aisleIndex)
            intercomAudio.PlayOneShot(aisleAnnouncementClips[aisleIndex]);

        yield return new WaitForSeconds(3f);

        dialogueText.text = "";

        if (intercomPanel != null)
            intercomPanel.SetActive(false);

        intercomCam.Priority = 0;
        mainCam.Priority = 10;

        if (cosmicManager != null)
            cosmicManager.isPaused = false;
    }

    public static void MarkInteractionComplete(string id)
    {
        completedInteractions.Add(id);
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