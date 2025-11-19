using UnityEngine;

public class SolarFlare : MonoBehaviour
{
    public bool solarFlareOn;
    public float solarTimer = 3f;
    public GameObject solarWhiteScreen;
    public Animator solarAnimator;
    public float solarTimeDelay = 1f;
    public bool solarFlareOff;

    public AudioSource solarBang;
    public AudioClip helloThere;
    public bool solarActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //Turns On the Solar Flare
        if (solarFlareOn)
        {
            solarActive = true;
            solarTimer -= Time.deltaTime;
            solarWhiteScreen.SetActive(true);
            solarAnimator.SetTrigger("SolarOn");
        }
        //Sets parameters for the solar flare to turn off and reset the timer
        if (solarTimer < 0)
        {
            solarTimer = 3f;
            solarFlareOn = false;
            solarAnimator.SetTrigger("SolarOff");
            solarFlareOff = true;

        }

        if (solarFlareOff)
        {
            solarTimeDelay -= Time.deltaTime;
            if (solarTimeDelay < 0)
            {
                solarWhiteScreen.SetActive(false);
                solarAnimator.SetTrigger("SolarOver");
                solarTimeDelay = 1f;
                solarFlareOff = false;
                solarActive = false;
            }
        }
    }

    public void SolarFlareBlast()
    {
        if (!solarActive)
        {
            solarFlareOn = true;
            solarBang.PlayOneShot(helloThere);
        }

    }
}