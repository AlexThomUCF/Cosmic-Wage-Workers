using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AlarmSequenceManager : MonoBehaviour
{
    public static AlarmSequenceManager Instance;

    [Header("Alarm Setup")]
    public AlarmNode[] alarms;          // size = 7
    public float revealDelay = 1f;
    public float timePerAlarm = 2f;

    [Header("Sequence Settings")]
    public int startSequenceLength = 3;
    public int maxSequenceLength = 5;

    [Header("Breathers")]
    public float initialStartDelay = 10f;
    public float successCooldown = 10f;
    public float failCooldown = 2.5f;

    [Header("Failure Pressure")]
    [SerializeField] private int maxFailures = 3;

    [Header("Spawn Control")]
    [SerializeField] private PropManager propManager;

    [Header("End Minigame")]
    public string sceneToLoadAfterGame;
    public float endDelay = 1.5f;

    [HideInInspector]
    public int currentSequenceIndex = 0;


    private int consecutiveFailures = 0;

    public List<int> currentSequence = new List<int>();
    private List<int> remainingAlarms = new List<int>();

    private int currentSequenceLength;
    public bool acceptingInput = false;
    private float timer;

    // ================================
    // UNITY LIFECYCLE
    // ================================

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        currentSequenceLength = startSequenceLength;
    }

    void Start()
    {
        StartCoroutine(InitialDelay());
    }

    void Update()
    {
        if (!acceptingInput)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Debug.Log("[SEQUENCE] FAILED — time ran out");
            acceptingInput = false;
            OnSequenceFailed();
        }
    }

    // ================================
    // SEQUENCE FLOW
    // ================================

    IEnumerator InitialDelay()
    {
        ResetAllAlarms();
        acceptingInput = false;

        Debug.Log("[SEQUENCE] Waiting before first sequence");

        yield return new WaitForSeconds(initialStartDelay);

        StartNewSequence();
    }

    public void StartNewSequence()
    {
        BuildNewSequence();
        StartCoroutine(RevealSequence());
    }

    void BuildNewSequence()
    {
        currentSequence.Clear();

        List<int> availableIDs = new List<int>();
        for (int i = 0; i < alarms.Length; i++)
            availableIDs.Add(i);

        for (int i = 0; i < currentSequenceLength; i++)
        {
            int index = Random.Range(0, availableIDs.Count);
            currentSequence.Add(availableIDs[index]);
            availableIDs.RemoveAt(index);
        }

        Debug.Log($"[SEQUENCE] New sequence: {string.Join(", ", currentSequence)}");
    }

    IEnumerator RevealSequence()
    {
        acceptingInput = false;
        remainingAlarms.Clear();
        ResetAllAlarms();

        // --- REVEAL PHASE ---
        for (int i = 0; i < currentSequence.Count; i++)
        {
            int id = currentSequence[i];
            currentSequenceIndex = i;  // <-- Track current alarm
            alarms[id].Reveal();
            remainingAlarms.Add(id);

            yield return new WaitForSeconds(revealDelay);
        }

        // --- PLAYER PHASE ---
        currentSequenceIndex = 0; // reset to first for gameplay
        foreach (int id in currentSequence)
            alarms[id].SetActive();

        timer = currentSequence.Count * timePerAlarm;
        acceptingInput = true;

        Debug.Log("[SEQUENCE] Player input enabled");
    }
    // ================================
    // HIT REGISTRATION
    // ================================

    public void RegisterHit(int id)
    {
        if (!acceptingInput || remainingAlarms.Count == 0)
            return;

        int expectedID = remainingAlarms[0];

        if (id == expectedID)
        {
            remainingAlarms.RemoveAt(0);

            if (remainingAlarms.Count == 0)
            {
                acceptingInput = false;
                OnSequenceSuccess();
            }
        }
        else
        {
            acceptingInput = false;
            OnSequenceFailed();
        }
    }

    // ================================
    // SUCCESS / FAILURE
    // ================================

    void OnSequenceSuccess()
    {
        Debug.Log("[SEQUENCE] COMPLETED");

        // NEW — relieve pressure
        consecutiveFailures = Mathf.Max(0, consecutiveFailures - 1);
        ApplySpawnPressure();

        if (currentSequenceLength < maxSequenceLength)
        {
            currentSequenceLength++;
            StartCoroutine(SuccessCooldown());
        }
        else
        {
            Debug.Log("[SEQUENCE] ALL SEQUENCES COMPLETE — MINIGAME DONE");

            StopAllCoroutines();
            Invoke(nameof(LoadNextScene), endDelay);
        }

    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoadAfterGame))
        {
            SceneManager.LoadScene(sceneToLoadAfterGame);
        }
        else
        {
            Debug.LogWarning("Scene name not set in AlarmSequenceManager.");
        }
    }


    void OnSequenceFailed()
    {
        // NEW — increase pressure, capped
        consecutiveFailures = Mathf.Min(consecutiveFailures + 1, maxFailures);
        ApplySpawnPressure();

        StartCoroutine(FailBreather());
    }

    IEnumerator SuccessCooldown()
    {
        ResetAllAlarms();
        acceptingInput = false;

        yield return new WaitForSeconds(successCooldown);

        StartNewSequence();
    }

    IEnumerator FailBreather()
    {
        ResetAllAlarms();
        acceptingInput = false;

        yield return new WaitForSeconds(failCooldown);

        StartNewSequence();
    }

    // ================================
    // PRESSURE HANDLING
    // ================================

    void ApplySpawnPressure()
    {
        float pressure = (float)consecutiveFailures / maxFailures;
        Debug.Log($"[SPAWN] Pressure level {consecutiveFailures}/{maxFailures} ({pressure})");
        propManager.SetPressureNormalized(pressure);

        // Hook into PropManager here:
        // propManager.SetPressureNormalized(pressure);
    }

    // ================================
    // HELPERS
    // ================================

    void ResetAllAlarms()
    {
        foreach (AlarmNode alarm in alarms)
            alarm.SetIdle();
    }

}
