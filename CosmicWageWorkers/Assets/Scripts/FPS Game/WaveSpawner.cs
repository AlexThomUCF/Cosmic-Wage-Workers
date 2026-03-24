using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SceneLoader loader;

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

    [Header("Lose Settings")]
    public string loseSceneName = "GameOver";

    [Header("Customer Interaction ID")]
    public string interactionID;

    private bool gameEnded = false;

    void Awake()
    {
        loader = FindFirstObjectByType<SceneLoader>();
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
        gameEnded = true;

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "FPSMainScene")
        {
            FindFirstObjectByType<ReturnMainMenu>().ReturnToMainMenu();
            return;
        }

        if (!string.IsNullOrEmpty(interactionID))
        {
            CustomerManager.MarkInteractionComplete(interactionID);
        }

        FinalMiniGame.miniGameCount++;
        loader.LoadSceneByName(nextSceneName);
    }

    void LoseGame()
    {
        gameEnded = true;
        loader.LoadSceneByName(loseSceneName);
    }
}