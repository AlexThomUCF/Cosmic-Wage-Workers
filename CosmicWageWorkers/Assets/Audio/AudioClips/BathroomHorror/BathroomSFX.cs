using UnityEngine;

public class BathroomSFX : MonoBehaviour
{
    public AudioSource bathSource;

    [Header("*** Audio Clips ***")]
    public AudioClip doorOpen;
    public AudioClip doorClose;
    public AudioClip doorBang;
    public AudioClip sectionOpen;
    public AudioClip waveStarted;

    [Header("*** Audio Background ***")]
    public AudioSource backgroundNoise;
    public AudioSource distortedStoreMusic;
    public AudioSource bugNoises;
    public AudioSource crawlNoise;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        distortedStoreMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDoorBang()
    {
        bathSource.PlayOneShot(doorBang);
    } 

    public void PlayDoorOpen()
    {
        bathSource.PlayOneShot(doorOpen);
    }

    public void PlayDoorClose()
    {
        bathSource.PlayOneShot(doorClose);
    }

    public void PlaySectionOpen()
    {
        bathSource.PlayOneShot(sectionOpen);
    }   

    public void StartMusic()
    {
        backgroundNoise.Play();
    }

    public void StopMusic()     
    {
        backgroundNoise.Stop();
    }

    public void StopDistortedMusic()
    {
        distortedStoreMusic.Stop();
    }

    public void PlayBugNoises()
    {
        bugNoises.Play();
    }

    public void PlayCrawlNoises()
    {
        crawlNoise.Play();
    }

    public void WaveNoise()
    {
        bathSource.PlayOneShot(waveStarted);
    }

    public void StopAllMusic()
    {
        backgroundNoise.Stop();
        distortedStoreMusic.Stop();
        bugNoises.Stop();
        crawlNoise.Stop();
    }
}
