using UnityEngine;

public class BathroomSFX : MonoBehaviour
{
    public AudioSource bathSource;
    public AudioClip doorOpen;
    public AudioClip doorClose;
    public AudioClip sectionOpen;
    public AudioSource backgroundNoise;
    public AudioSource distortedStoreMusic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        distortedStoreMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void StartStoreMusic()
    {

    }
}
