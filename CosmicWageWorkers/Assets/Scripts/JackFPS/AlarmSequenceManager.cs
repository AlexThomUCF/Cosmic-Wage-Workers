using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AlarmSequenceManager : MonoBehaviour
{
    public static AlarmSequenceManager Instance;

    [Header("Alarm Setup")]
    public AlarmNode[] alarms;
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

    private int consecutiveFailures = 0;

    private List<int> currentSequence = new List<int>();
    private List<int> remainingAlarms = new List<int>();

    private int currentSequenceLength;
    public bool acceptingInput = false;
    private float timer;

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
            Debug.Log("SEQUENCE FAILED — time ran out");
            acceptingInput = false;
            OnSequenceFailed();
        }
    }

    IEnumerator InitialDelay()
    {
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

        Debug.Log("New sequence: " + string.Join(", ", currentSequence));
    }

    IEnumerator RevealSequence()
    {
        acceptingInput = false;
        remainingAlarms.Clear();
        ResetAllAlarms();

        foreach (int id in currentSequence)
        {
            alarms[id].Reveal();
            remainingAlarms.Add(id);
            yield return new WaitForSeconds(revealDelay);
        }

        foreach (int id in currentSequence)
            alarms[id].SetActive();

        timer = currentSequence.Count * timePerAlarm;
        acceptingInput = true;
    }

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

    void OnSequenceSuccess()
    {
        consecutiveFailures = Mathf.Max(0, consecutiveFailures - 1);
        ApplySpawnPressure();

        if (currentSequenceLength < maxSequenceLength)
        {
            currentSequenceLength++;
            StartCoroutine(SuccessCooldown());
        }
        else
        {
            StopAllCoroutines();
            Invoke(nameof(LoadNextScene), endDelay);
        }
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoadAfterGame))
            SceneManager.LoadScene(sceneToLoadAfterGame);
    }

    void OnSequenceFailed()
    {
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

    void ApplySpawnPressure()
    {
        float pressure = (float)consecutiveFailures / maxFailures;
        propManager.SetPressureNormalized(pressure);
    }

    void ResetAllAlarms()
    {
        foreach (AlarmNode alarm in alarms)
            alarm.SetIdle();
    }
}