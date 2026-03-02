using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class LoadTimer : MonoBehaviour
{
    private Stopwatch stopwatch;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        stopwatch = new Stopwatch();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;

        stopwatch.Start();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene current, Scene next)
    {
        stopwatch.Reset();
        stopwatch.Start();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        stopwatch.Stop();

        UnityEngine.Debug.Log(
        $"[Diagnostics] Scene '{scene.name}' loaded in {stopwatch.Elapsed.TotalSeconds:F3} seconds ({stopwatch.ElapsedMilliseconds} ms)"
        );
    }
}
