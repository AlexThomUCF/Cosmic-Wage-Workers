using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MiniGameTimer : MonoBehaviour
{
    public static MiniGameTimer Instance;

    public float timeLimit = 20f;
    public TextMeshProUGUI timerText;
    public GameObject timerUIRoot;

    private float timeRemaining;
    private bool running = false;
    private bool finished = false;
    private Coroutine timerRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        //StartTimer();
    }

    public void StartTimer()
    {
        timeRemaining = timeLimit;
        running = true;
        finished = false;

        if (timerUIRoot != null)
            timerUIRoot.SetActive(true);

        if (timerRoutine != null)
            StopCoroutine(timerRoutine);

        timerRoutine = StartCoroutine(TimerRoutine());
    }

    public void StopTimer()
    {
        running = false;
        finished = true;

        if (timerUIRoot != null)
            timerUIRoot.SetActive(false);
    }

    private IEnumerator TimerRoutine()
    {
        while (running && !finished && timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;

            if (timerText != null)
            {
                int seconds = Mathf.CeilToInt(Mathf.Max(0f, timeRemaining));
                timerText.text = "Time Left: " + seconds;
            }

            yield return null;
        }

        if (!finished && timeRemaining <= 0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}