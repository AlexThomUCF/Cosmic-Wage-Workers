using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

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

    [Header("Sequence Feedback Audio")]
    public AudioSource sequenceAudio;
    public AudioClip successClip;
    public AudioClip failClip;

    [Header("UI")]
    public TextMeshProUGUI clearGroceriesText;
    public UIFlashMessage clearGroceriesMessage;

    [Header("Customer Interaction ID")]
    public string interactionID;

    private int consecutiveFailures = 0;

    private List<int> currentSequence = new List<int>();
    private List<int> remainingAlarms = new List<int>();

    private int currentSequenceLength;
    public bool acceptingInput = false;
    private float sequenceTimer;


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

        sequenceTimer -= Time.deltaTime;

        if (sequenceTimer <= 0f)
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

        sequenceTimer = currentSequence.Count * timePerAlarm;
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
        if (sequenceAudio && successClip)
            sequenceAudio.PlayOneShot(successClip);

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

            Debug.Log("Final sequence completed. Stopping prop spawns.");

            if (propManager != null)
                propManager.spawningEnabled = false;
                
            if (clearGroceriesText != null)
                clearGroceriesText.gameObject.SetActive(true);
            if (clearGroceriesMessage != null)
            {
                clearGroceriesMessage.FlashMessage();
            }

            StartCoroutine(WaitForRemainingProps());
        }
    }
    
    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoadAfterGame))
            SceneManager.LoadScene(sceneToLoadAfterGame);
    }

        void OnSequenceFailed()
    {
        if (sequenceAudio && failClip)
            sequenceAudio.PlayOneShot(failClip);

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
        sequenceTimer = 0f;

        foreach (AlarmNode alarm in alarms)
            alarm.SetIdle();
    }

        IEnumerator WaitForRemainingProps()
    {
        Debug.Log("Waiting for remaining props to be cleared...");

        while (GameObject.FindObjectsOfType<PropAi>().Length > 0)
        {
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("All props cleared. Ending game.");

        yield return new WaitForSeconds(endDelay);

        if (!string.IsNullOrEmpty(interactionID))
        {
            CustomerManager.MarkInteractionComplete(interactionID);
        }

        FinalMiniGame.miniGameCount++;
        LoadNextScene();
    }

        public float GetTimerPercent()
    {
        if (!acceptingInput)
            return 1f;

        float maxTime = currentSequence.Count * timePerAlarm;
        return sequenceTimer / maxTime;
    }
}