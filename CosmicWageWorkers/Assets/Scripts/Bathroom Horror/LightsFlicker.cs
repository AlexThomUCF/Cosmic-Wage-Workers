using UnityEngine;

public class LightsFlicker : MonoBehaviour
{
    public GameObject secondSpotLight1;
    public GameObject secondSpotLight2;
    public GameObject thirdSpotLight1;
    public GameObject thirdSpotLight2;
    public GameObject fourthSpotLight1;
    public GameObject fourthSpotLight2;

    private float lightFlickerTimer = 0.4f;
    private bool lightsOn = true;

    public BathroomsUnhide bathroomUnhide;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lightFlickerTimer -= Time.deltaTime;
        if (lightFlickerTimer <= 0f && bathroomUnhide.horrorGameStarted && bathroomUnhide.doorOpened == false)
        {
            LightsOnAndOff();
            lightFlickerTimer = 0.4f;
        }

        if (bathroomUnhide.doorOpened)
        {
            secondSpotLight1.SetActive(true);
            secondSpotLight2.SetActive(true);
            thirdSpotLight1.SetActive(true);
            thirdSpotLight2.SetActive(true);
            fourthSpotLight1.SetActive(true);
            fourthSpotLight2.SetActive(true);
        }
    }

    public void LightsOnAndOff()
    {

        lightsOn = !lightsOn;

        if (lightsOn)
        {
            secondSpotLight1.SetActive(false);
            secondSpotLight2.SetActive(false);
            thirdSpotLight1.SetActive(false);
            thirdSpotLight2.SetActive(false);
            fourthSpotLight1.SetActive(false);
            fourthSpotLight2.SetActive(false);

        }
        else
        {

            secondSpotLight1.SetActive(true);
            secondSpotLight2.SetActive(true);
            thirdSpotLight1.SetActive(true);
            thirdSpotLight2.SetActive(true);
            fourthSpotLight1.SetActive(true);
            fourthSpotLight2.SetActive(true);
        }
    }
}
