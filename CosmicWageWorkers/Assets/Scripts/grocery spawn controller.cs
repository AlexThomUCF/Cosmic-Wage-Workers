using UnityEngine;

public class StartSpawnOnDisable : MonoBehaviour
{
    [SerializeField] private PropManager propManager;

    private void OnDisable()
    {
        if (!Application.isPlaying) return;
        if (propManager != null) propManager.StartSpawning();
    }
}