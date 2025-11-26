using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;

    [Header("UI References (Player Only)")]
    [SerializeField] private TextMeshProUGUI currentLapTimeText;
    [SerializeField] private TextMeshProUGUI bestLapTimeText;
    [SerializeField] private TextMeshProUGUI overallRaceTimeText;
    [SerializeField] private TextMeshProUGUI lapText;
    [SerializeField] private TextMeshProUGUI checkpointMissedText;

    [Header("Race Settings")]
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private bool isCircuit = false;
    [SerializeField] private int totalLaps = 1;

    [Header("Global State")]
    public static bool raceDone = false;
    public static bool playerCameFirst = false;

    [Header("UI settings")]
    public GameObject winScreen;
    public GameObject loseScreen;
    
    [Header("Customer Interaction ID")]
    public string interactionID; // Assign the ID of the customer for this race

    private Dictionary<GameObject, RacerProgress> racers = new Dictionary<GameObject, RacerProgress>();

    private bool ifCheckpointMissed = false;

    [SerializeField] SceneLoader loader;

    #region Unity Functions
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        loader = FindFirstObjectByType<SceneLoader>();
    }

    private void Update()
    {
        UpdateTimers();
        UpdateUI();
    }
    #endregion

    #region Racer Registration
    public void RegisterRacer(GameObject racer) // Add Trigger to Ai racer and Tag of any kind to register it automatically
    {
        if (!racers.ContainsKey(racer))
        {
            RacerProgress data = new RacerProgress();
            data.racerName = racer.name;
            racers.Add(racer, data);
        }
    }
    #endregion

    #region Checkpoint Management
    public void CheckpointReached(GameObject racer, int checkpointIndex)
    {
        if (!racers.ContainsKey(racer))
            RegisterRacer(racer);

        RacerProgress data = racers[racer];

        if ((!data.raceStarted && checkpointIndex != 0) || data.raceFinished) return;

        if (checkpointIndex == data.lastCheckpointIndex + 1)
        {
            UpdateCheckpoint(racer, checkpointIndex);
            HideCheckpointMissedText(racer);
        }
        else
        {
            bool validLapFinish = isCircuit && data.raceStarted && data.lastCheckpointIndex == checkpoints.Length - 1 && checkpointIndex == 0;
            if (validLapFinish)
            {
                HideCheckpointMissedText(racer);
                UpdateCheckpoint(racer, checkpointIndex);
            }
            else
            {
                ShowCheckpointMissedText(racer);
            }
        }
    }

    private void UpdateCheckpoint(GameObject racer, int checkpointIndex)
    {
        RacerProgress data = racers[racer];

        if (checkpointIndex == 0)
        {
            if (!data.raceStarted)
            {
                StartRace(racer);
            }
            else
            {
                OnLapFinish(racer);
            }
        }
        else if (!isCircuit && checkpointIndex == checkpoints.Length - 1)
        {
            OnLapFinish(racer);
        }

        data.lastCheckpointIndex = checkpointIndex;
    }
    #endregion

    #region Race Management
    private void StartRace(GameObject racer)
    {
        RacerProgress data = racers[racer];
        data.raceStarted = true;
        data.raceFinished = false;
    }

    private void OnLapFinish(GameObject racer)
    {
        RacerProgress data = racers[racer];

        if (data.currentLapTime < data.bestLapTime)
            data.bestLapTime = data.currentLapTime;

        data.currentLap++;

        if (data.currentLap > totalLaps)
        {
            EndRace(racer);
        }
        else
        {
            data.currentLapTime = 0f;
            data.lastCheckpointIndex = isCircuit ? 0 : -1;
        }
    }

    private void EndRace(GameObject racer)
    {
        RacerProgress data = racers[racer];
        data.raceFinished = true;
        data.raceStarted = false;

        Debug.Log($"{data.racerName} finished the race in {FormatTime(data.overallRaceTime)}!");

        // Count how many racers have finished
        int finishedCount = 0;
        foreach (var r in racers.Values)
        {
            if (r.raceFinished)
                finishedCount++;
        }

        // If this is the first to finish...
        if (finishedCount == 1)
        {
            if (racer.CompareTag("Player"))
            {
                playerCameFirst = true;
                Debug.Log("Player came in FIRST!");

                // Mark the customer interaction complete
                if (!string.IsNullOrEmpty(interactionID))
                {
                    CustomerManager.MarkInteractionComplete(interactionID);
                }
            }
            else
            {
                playerCameFirst = false;
                Debug.Log("Player did NOT come first.");
            }
        }

        // Check if all racers are done
        bool allDone = true;
        foreach (var r in racers.Values)
        {
            if (!r.raceFinished)
            {
                allDone = false;
                break;
            }
        }

        if (allDone)
        {
            raceDone = true;
            Debug.Log("All racers have finished the race!");
            if (allDone && playerCameFirst)
            {
                Debug.Log("Trigger end scene");
                winScreen.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }
            else if (allDone && !playerCameFirst)
            {
                Debug.Log("Replay the game");
                loseScreen.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    #endregion

    #region Timer & UI
    private void UpdateTimers()
    {
        foreach (var kvp in racers)
        {
            RacerProgress data = kvp.Value;
            if (data.raceStarted && !data.raceFinished)
            {
                data.currentLapTime += Time.deltaTime;
                data.overallRaceTime += Time.deltaTime;
            }
        }
    }

    private void UpdateUI()
    {
        // Only update UI for the player (assuming the player GameObject has tag "Player")
        if (!racers.ContainsKey(GameObject.FindGameObjectWithTag("Player"))) return;

        RacerProgress playerData = racers[GameObject.FindGameObjectWithTag("Player")];

        currentLapTimeText.text = FormatTime(playerData.currentLapTime);
        bestLapTimeText.text = FormatTime(playerData.bestLapTime);
        overallRaceTimeText.text = FormatTime(playerData.overallRaceTime);
        lapText.text = $"Lap: {playerData.currentLap}/{totalLaps}";

        UpdateCheckpointMissedText();
    }

    private void UpdateCheckpointMissedText()
    {
        if (ifCheckpointMissed)
        {
            float alpha = Mathf.PingPong(Time.time * 2, 1);
            Color newColor = checkpointMissedText.color;
            newColor.a = alpha;
            checkpointMissedText.color = newColor;
        }
    }

    private void ShowCheckpointMissedText(GameObject racer)
    {
        if (racer.CompareTag("Player"))
        {
            if (!ifCheckpointMissed)
            {
                checkpointMissedText.gameObject.SetActive(true);
                ifCheckpointMissed = true;
            }
        }
    }

    private void HideCheckpointMissedText(GameObject racer)
    {
        if (racer.CompareTag("Player"))
        {
            if (ifCheckpointMissed)
            {
                checkpointMissedText.gameObject.SetActive(false);
                ifCheckpointMissed = false;
            }
        }
    }
    #endregion

    public void LoseCon()
    {
        // Get the name of the currently active scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Load the scene using its name
        SceneManager.LoadScene(currentSceneName);
    }

    public void WinCon()
    {
        string mainSceneName = "POCScene";
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        loader.LoadSceneByName(mainSceneName);
        //SceneManager.LoadScene(mainSceneName);
    }
    public void ExitToMainScene()
    {
        string mainSceneName = "POCScene";
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // SceneManager.LoadScene(mainSceneName);
        loader.LoadSceneByName(mainSceneName);

        //Add save, save that this game completed. 

    }

    #region Utility
    private string FormatTime(float time)
    {
        if (float.IsInfinity(time) || time < 0) return "--:--";
        int minutes = (int)time / 60;
        float seconds = time % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    #endregion
}

[System.Serializable]
public class RacerProgress
{
    public string racerName;
    public int lastCheckpointIndex = -1;
    public int currentLap = 1;
    public float currentLapTime = 0f;
    public float bestLapTime = Mathf.Infinity;
    public float overallRaceTime = 0f;
    public bool raceStarted = false;
    public bool raceFinished = false;
}
