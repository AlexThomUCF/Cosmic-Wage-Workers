using UnityEngine;

public class RandomPlatformGroup : MonoBehaviour
{
    [SerializeField] private RandomPlatform[] platforms = new RandomPlatform[3];
    private CheckpointSystem checkpointSystem;

    private void Start()
    {
        checkpointSystem = FindObjectOfType<CheckpointSystem>();
        RandomizeSelection();
    }

    public void RandomizeSelection()
    {
        // Reset all platforms to be visible
        foreach (RandomPlatform platform in platforms)
        {
            platform.ReappearPlatform();
        }

        // Pick a random safe platform (0, 1, or 2)
        int safePlatformIndex = Random.Range(0, platforms.Length);

        // Set all platforms - 1 safe, 2 unsafe
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i].SetSafe(i == safePlatformIndex);
        }
    }
}
