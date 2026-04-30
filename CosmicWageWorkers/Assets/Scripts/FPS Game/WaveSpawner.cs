using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;


public class WaveSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SceneLoader loader;
    [SerializeField] private GameObject player; 


    [Header("Spawning")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Wave Settings")]
    public float timeBetweenEnemies = 0.5f;
    public float timeBetweenWaves = 3f;
    public int totalWaves = 10;

    [Header("Scaling (Exponential Growth)")]
    public int baseEnemies = 5;
    public float growthMultiplier = 1.5f;
    public int maxEnemiesPerWave = 100;

    private int currentWave = 0;
    private int enemiesAlive = 0;

    [Header("Win Settings")]
    public string nextSceneName = "ProgramPrototype";
    public GameObject orbitDirector;
    public PlayableDirector endCineDirector;
    public GameObject fpsPlayer;
    public GameObject secondPlayer;
    public GameObject weapons;
    public GameObject ui1;
    public GameObject ui2;


    [Header("Lose Settings")]
    public string loseSceneName = "GameOver";

    [Header("UI")]
    public TextMeshProUGUI waveText;

    [Header("Customer Interaction ID")]
    public string interactionID;

    private bool gameEnded = false;

    void Awake()
    {
        loader = FindFirstObjectByType<SceneLoader>();
        endCineDirector.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        if (endCineDirector != null)
            endCineDirector.stopped += OnTimelineFinished;
    }

    void OnDisable()
    {
        if (endCineDirector != null)
            endCineDirector.stopped -= OnTimelineFinished;
    }

    void OnTimelineFinished(PlayableDirector director)
    {
        if (director == endCineDirector)
        {
            endCineDirector.gameObject.SetActive(false);
           
            StartCoroutine(MovePlayerAfterTimeline());
        }

    }
    IEnumerator MovePlayerAfterTimeline()
    {
        //endCineDirector.gameObject.SetActive(false);
        //orbitDirector.SetActive(false);


        yield return null;
        yield return new WaitForEndOfFrame();

        Camera.main.transform.position = fpsPlayer.transform.position;

        Rigidbody rb = fpsPlayer.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // Unity 6
            rb.angularVelocity = Vector3.zero;
            rb.position = secondPlayer.transform.position;
            rb.rotation = secondPlayer.transform.rotation;
        }
        else
        {
            fpsPlayer.transform.position = secondPlayer.transform.position;
            fpsPlayer.transform.rotation = secondPlayer.transform.rotation;
        }


        yield return null;

        
    }

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(1f);

        while (currentWave < totalWaves)
        {
            currentWave++;

            yield return StartCoroutine(SpawnWave());

            // ? WAIT until all enemies are dead
            while (enemiesAlive > 0)
            {
                yield return null;
            }

            // Optional delay before next wave starts
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        WinGame();
    }

    IEnumerator SpawnWave()
    {
        int enemiesThisWave = Mathf.Min(
            Mathf.RoundToInt(baseEnemies * Mathf.Pow(growthMultiplier, currentWave - 1)),
            maxEnemiesPerWave
        );

        Debug.Log("Wave " + currentWave + " spawning " + enemiesThisWave + " enemies");

        waveText.text = "Wave " + currentWave + " / " + totalWaves;

        for (int i = 0; i < enemiesThisWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // OPTIONAL: link spawner to EnemyBase (cleaner than Find)
        EnemyBase baseScript = enemy.GetComponent<EnemyBase>();
        if (baseScript != null)
        {
            baseScript.SetSpawner(this);
        }

        enemiesAlive++;
    }

    // Called when enemy dies
    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }

    // Optional: if you ever use "reach end" logic
    public void OnEnemyReachedEnd()
    {
        if (!gameEnded)
            LoseGame();
    }

    void WinGame()
    {
        if (gameEnded) return; 
        gameEnded = true;

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "FPSMainScene")
        {
            endCineDirector.gameObject.SetActive(true);
            weapons.SetActive(false);
            ui1.SetActive(false);
            ui2.SetActive(false);
            endCineDirector.Play();
            

        }

        if (!string.IsNullOrEmpty(interactionID))
        {
            CustomerManager.MarkInteractionComplete(interactionID);
        }

        FinalMiniGame.miniGameCount++;
        //loader.LoadSceneByName(nextSceneName);
    }

    void LoseGame()
    {
        gameEnded = true;
        loader.LoadSceneByName(loseSceneName);
    }
}