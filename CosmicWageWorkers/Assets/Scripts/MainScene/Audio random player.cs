using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomAudioPlayer : MonoBehaviour
{
    public AudioClip[] clips;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayLoop());
    }

    System.Collections.IEnumerator PlayLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(6f, 12f));
            int index = Random.Range(0, clips.Length);
            audioSource.PlayOneShot(clips[index]);
        }
    }
}