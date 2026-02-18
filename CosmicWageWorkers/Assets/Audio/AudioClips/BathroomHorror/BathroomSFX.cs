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
    public AudioClip hey;
    public AudioClip getOut;
    public AudioClip jumpScareSound;
    public AudioClip heavyBreathing;


    [Header("*** Audio Background ***")]
    public AudioSource backgroundNoise;
    public AudioSource distortedStoreMusic;
    public AudioSource heartBeat;
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

 

    public void StopAllMusic()
    {
        backgroundNoise.Stop();
        distortedStoreMusic.Stop();
        bugNoises.Stop();
        crawlNoise.Stop();
        heartBeat.Stop();
    }
}
