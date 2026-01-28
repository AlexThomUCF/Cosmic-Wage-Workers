using UnityEngine;

public class BathroomsUnhide : MonoBehaviour
{
    public GameObject player;

    public GameObject blockOut;

    public GameObject firstWall;

    public GameObject firstPuddle;

    public GameObject firstMop;

    public GameObject firstSection;

    public GameObject firstLight;

    public GameObject secondWall;

    public GameObject secondLight;

    public GameObject secondMop;

    public GameObject secondPuddle;

    public GameObject secondSection;

    public GameObject thirdWall;

    public GameObject thirdMop;

    public GameObject thirdLight;

    public GameObject thirdPuddle;

    public GameObject thirdSection;

    public GameObject fourthLight;

    public GameObject lastLight;

    public GameObject lastMop;

    public Animator doorAnimator;

    public GameObject lastSoup;

    public BathroomSFX bathroomSFX;

    private bool horrorGameStarted = true; 
    private bool doorOpened = false;

    private bool firstSectionOpened = true;
    private bool secondSectionOpened = false;
    private bool thirdSectionOpened = false;

    public float doorTimer;

    public float doorCloseDelay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.z > 341 && firstSectionOpened)
        {
            FirstSection();
        }

        if (player.transform.position.z > 356 && secondSectionOpened)
        {
            SecondSection();
        }

        if (player.transform.position.z > 387 && thirdSectionOpened)
        {
            ThirdSection();
        }

        if (doorOpened)
        {
            doorTimer -= Time.deltaTime;
            if (doorTimer <= 0)
            {
                CloseDoor();
                doorCloseDelay -= Time.deltaTime;
                if (doorCloseDelay <= 0)
                {
                    ReturnToBathroom();
                }

            }
        }
    }

    public void OpenDoor()
    {
        bathroomSFX.StopMusic();
        doorAnimator.SetTrigger("DoorOpen");
        doorOpened = true;
        bathroomSFX.PlayDoorOpen();
    }   

    public void CloseDoor()
    {
        doorAnimator.SetTrigger("DoorClosed");
        bathroomSFX.PlayDoorClose();
    }

    private void ReturnToBathroom()
    {
        firstSection.SetActive(false);
        secondSection.SetActive(false);
        thirdSection.SetActive(false);
        fourthLight.SetActive(false);
        firstWall.SetActive(true);
        firstLight.SetActive(true);
        lastLight.SetActive(true);
        player.transform.position = new Vector3(-111f, 5f, 332f);
        doorOpened = false;
        horrorGameStarted = false;
        bathroomSFX.StartStoreMusic();
        lastSoup.SetActive(true);
        lastMop.SetActive(true);

    }

    private void FirstSection()
    {
        firstWall.SetActive(false);
        firstPuddle.SetActive(false);
        firstLight.SetActive(false);
        secondLight.SetActive(true);
        firstSection.SetActive(true);
        firstMop.SetActive(false);
        bathroomSFX.StopStoreMusic();
        bathroomSFX.PlaySectionOpen();
        bathroomSFX.StartMusic();   
        firstSectionOpened = false;
        secondSectionOpened = true;

    }

    private void SecondSection()
    {
        secondWall.SetActive(false);
        secondPuddle.SetActive(false);
        secondLight.SetActive(false);
        thirdLight.SetActive(true);
        secondSection.SetActive(true);
        secondMop.SetActive(false);
        bathroomSFX.PlaySectionOpen();
        secondSectionOpened = false;
        thirdSectionOpened = true;
    }

    private void ThirdSection()
    {
        thirdWall.SetActive(false);
        thirdPuddle.SetActive(false);
        thirdLight.SetActive(false);
        fourthLight.SetActive(true);
        thirdSection.SetActive(true);
        thirdMop.SetActive(false); 
        bathroomSFX.PlaySectionOpen();
        thirdSectionOpened = false;
    }

    public void BackToMainScene()
    {
        if (!horrorGameStarted)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }
    }

}
