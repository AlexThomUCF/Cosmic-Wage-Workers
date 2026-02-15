using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource source;

    [Header("Clips")]
    public AudioClip handHitOccupied;
    public AudioClip falling;
    public AudioClip hitGround;
    public AudioClip warning;
    public AudioClip hitByItem;

    void Awake()
    {
        if (source == null)
            source = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
            return;

        source.PlayOneShot(clip, volume);
    }
}
