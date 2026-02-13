using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSequenceManager : MonoBehaviour
{
    public AlarmNode[] alarms; // total = 7
    public FPSManager fpsManager;

    [Header("Sequence")]
    public float revealDelay = 1f;       // time between alarm activations
    public float timePerAlarm = 1.5f;     // action phase time

    private int[] sequenceLengths = { 3, 4, 5 };
    private int currentStage = 0;

    private List<int> currentSequence = new();
    private HashSet<int> remainingAlarms = new();

    private float timer;
    private bool acceptingInput = false;

    void Start()
    {
        StartNextSequence();
    }

    void Update()
    {
        if (!acceptingInput) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SequenceFailed();
        }
    }

   void StartNextSequence()
    {
        if (currentStage >= sequenceLengths.Length)
        {
            Debug.Log("[ALARM] All alarm sequences completed.");
            return;
        }

        Debug.Log($"[ALARM] Starting sequence {currentStage + 1} " +
                $"(Length: {sequenceLengths[currentStage]})");

        ResetAllAlarms();
        BuildSequence(sequenceLengths[currentStage]);

        StartCoroutine(RevealSequence());
    }

    void BuildSequence(int length)
    {
        currentSequence.Clear();

        while (currentSequence.Count < length)
        {
            int id = Random.Range(0, alarms.Length);
            if (!currentSequence.Contains(id))
            {
                currentSequence.Add(id);
            }
        }
    }

    IEnumerator RevealSequence()
    {
        acceptingInput = false;
        remainingAlarms.Clear();

        foreach (int id in currentSequence)
        {
            alarms[id].SetActive();
            remainingAlarms.Add(id);
            yield return new WaitForSeconds(revealDelay);
        }

        // Reveal done → player phase starts
        timer = currentSequence.Count * timePerAlarm;
        acceptingInput = true;
    }

    public void RegisterHit(int alarmID)
    {
        if (!acceptingInput) return;
        if (!remainingAlarms.Contains(alarmID)) return;

        remainingAlarms.Remove(alarmID);
        alarms[alarmID].SetIdle();

        if (remainingAlarms.Count == 0)
        {
            SequenceSuccess();
        }
    }

    void SequenceSuccess()
    {
        acceptingInput = false;
        currentStage++;

        Debug.Log("[ALARM] Sequence SUCCESS → enemy pressure reduced");

        fpsManager.OnAlarmSuccess();
        Invoke(nameof(StartNextSequence), 1.5f);
    }

    void SequenceFailed()
    {
        acceptingInput = false;

        Debug.Log("[ALARM] Sequence FAILED → enemy pressure increased");

        fpsManager.OnAlarmFailure();
        Invoke(nameof(StartNextSequence), 1.5f);
    }

    void ResetAllAlarms()
    {
        foreach (AlarmNode alarm in alarms)
        {
            alarm.SetIdle();
        }
    }
}

