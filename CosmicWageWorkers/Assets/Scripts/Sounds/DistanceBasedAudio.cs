using UnityEngine;

public class DistanceBasedAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public PlayerMovement player;
    public float maxDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame

    private void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>();
    }
    void Update()
    {
        float distance = Vector3.Distance(transform.position,player.transform.position);
        float volume = Mathf.Clamp01(1 - (distance / maxDistance));

        audioSource.volume = volume;

    }
}
