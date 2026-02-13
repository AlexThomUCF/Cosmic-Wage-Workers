using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private List<int> currentSequence = new List<int>();
    private List<int> remainingAlarms = new List<int>();

    private int currentSequenceLength;
    private bool acceptingInput = false;
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
        Debug.Log("[SEQUENCE] Waiting before first sequence");

        ResetAllAlarms();
        acceptingInput = false;

        yield return new WaitForSeconds(initialStartDelay);

        StartNewSequence();
    }

    void StartNewSequence()
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

        // --- REVEAL PHASE (solid red, ordered) ---
        foreach (int id in currentSequence)
        {
            alarms[id].Reveal(); // solid red
            remainingAlarms.Add(id);
            yield return new WaitForSeconds(revealDelay);
        }

        // --- PLAYER PHASE (flicker enabled) ---
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
        if (!acceptingInput)
            return;

        if (remainingAlarms.Count == 0)
            return;

        int expectedID = remainingAlarms[0];

        if (id == expectedID)
        {
            remainingAlarms.RemoveAt(0);
            Debug.Log($"[SEQUENCE] Correct alarm hit: {id}");

            if (remainingAlarms.Count == 0)
            {
                acceptingInput = false;
                OnSequenceSuccess();
            }
        }
        else
        {
            Debug.Log("[SEQUENCE] FAILED — wrong order");
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
        DecreaseSpawnRate();

        if (currentSequenceLength < maxSequenceLength)
        {
            currentSequenceLength++;
            Debug.Log($"[SEQUENCE] Difficulty increased → length {currentSequenceLength}");
            StartCoroutine(SuccessCooldown());
        }
        else
        {
            Debug.Log("[SEQUENCE] ALL SEQUENCES COMPLETE — MINIGAME DONE");
            // Hook win condition here
        }
    }

    void OnSequenceFailed()
    {
        IncreaseSpawnRate();
        StartCoroutine(FailBreather());
    }

    IEnumerator SuccessCooldown()
    {
        ResetAllAlarms();
        acceptingInput = false;

        Debug.Log("[SEQUENCE] Success cooldown");

        yield return new WaitForSeconds(successCooldown);

        StartNewSequence();
    }

    IEnumerator FailBreather()
    {
        ResetAllAlarms();
        acceptingInput = false;

        Debug.Log("[SEQUENCE] Fail breather");

        yield return new WaitForSeconds(failCooldown);

        StartNewSequence();
    }

    // ================================
    // HELPERS
    // ================================

    void ResetAllAlarms()
    {
        foreach (AlarmNode alarm in alarms)
            alarm.SetIdle();
    }

    void IncreaseSpawnRate()
    {
        Debug.Log("[SPAWN] Spawn rate increased");
        // Hook into PropManager here
    }

    void DecreaseSpawnRate()
    {
        Debug.Log("[SPAWN] Spawn rate decreased");
        // Hook into PropManager here
    }
}


