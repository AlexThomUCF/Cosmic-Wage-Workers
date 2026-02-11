using UnityEngine;

public class GravitySFX : MonoBehaviour
{
    public AudioSource gravityAudioSource;
    public AudioSource clipAudioSource;
    public AudioClip fastFall;
    public AudioClip boost;
    public AudioClip obstacle;
    public AudioClip clickSFX;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gravityAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonSound()
    {
        clipAudioSource.PlayOneShot(clickSFX);
    }
}
