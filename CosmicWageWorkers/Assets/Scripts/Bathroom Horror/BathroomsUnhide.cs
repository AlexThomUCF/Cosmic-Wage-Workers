using UnityEngine;

public class BathroomsUnhide : MonoBehaviour
{
    public GameObject player;

    [Header("*** Obstacles ***")]
    public GameObject roach;
    public GameObject roach2;
    public GameObject blockOut;
    public GameObject firstWaveOfRoaches;
    public GameObject secondWaveOfRoaches;

    [Header("*** Walls ***")]
    public GameObject firstWall;
    public GameObject secondWall;
    public GameObject thirdWall;

    [Header("*** Mops ***")]
    public GameObject firstMop;
    public GameObject secondMop;
    public GameObject thirdMop;
    public GameObject lastMop;

    [Header("*** Sections ***")]
    public GameObject firstSection;
    public GameObject secondSection;
    public GameObject thirdSection;

    [Header("*** Puddles ***")]
    public GameObject firstPuddle;
    public GameObject secondPuddle;
    public GameObject thirdPuddle;
    public GameObject lastSoup;

    [Header("*** Lights ***")]
    public GameObject firstLight;
    public GameObject secondLight;
    public GameObject thirdLight;
    public GameObject fourthLight;
    public GameObject lastLight;


    [Header("*** Others ***")]
    public Animator doorAnimator;
    public BathroomSFX bathroomSFX;
    
    private bool horrorGameStarted = true; 
    private bool doorOpened = false;

    private bool firstSectionOpened = true;
    private bool secondSectionOpened = false;
    private bool thirdSectionOpened = false;
    private bool fourthSectionOpened = false;
    private bool canLeaveBathroom = false;
    private bool wave1started = false;
    private bool wave2started = false;

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

        if (player.transform.position.z > 410 && fourthSectionOpened)
        {
            WaveOne();
        }

        if (player.transform.position.z > 425 && wave1started && !wave2started)
        {
            WaveTwo();
        }

        if (thirdSectionOpened && player.transform.position.z < 350)
        {
            RestartLevel();
        }

        if (fourthSectionOpened && player.transform.position.z < 376)
        {
            RestartLevel();
        }

        if (wave1started && player.transform.position.z < 389)
        {
            RestartLevel();
        }

        if (wave2started && player.transform.position.z < 410)
        {
            RestartLevel();
        }





        if (doorOpened)
        {
            doorTimer -= Time.deltaTime;
            if (doorTimer <= 0)
            {
                CloseEventDoor();
                doorCloseDelay -= Time.deltaTime;
                if (doorCloseDelay <= 0)
                {
                    ReturnToBathroom();
                }

            }
        }

        if (lastSoup == null)
        {
            canLeaveBathroom = true;
        }
    }

    public void OpenEventDoor()
    {
        bathroomSFX.StopAllMusic();
        bathroomSFX.PlayDoorOpen();
        doorAnimator.SetTrigger("DoorOpen");
        doorOpened = true;
    }   

    public void CloseEventDoor()
    {
        bathroomSFX.PlayDoorClose();
        doorAnimator.SetTrigger("DoorClosed");
        blockOut.SetActive(false);      
    }


    private void ReturnToBathroom()
    {
        wave1started = false;
        wave2started = false;
        firstSection.SetActive(false);
        secondSection.SetActive(false);
        thirdSection.SetActive(false);
        fourthSectionOpened = false;
        fourthLight.SetActive(false);
        firstWall.SetActive(true);
        firstLight.SetActive(true);
        lastLight.SetActive(true);
        player.transform.position = new Vector3(-111f, 5f, 332f);
        doorOpened = false;
        horrorGameStarted = false;
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
        bathroomSFX.StopDistortedMusic();
        bathroomSFX.PlaySectionOpen();
        bathroomSFX.StartMusic();   
        firstSectionOpened = false;
        secondSectionOpened = true;
        blockOut.SetActive(true);

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
        bathroomSFX.PlayBugNoises();
        secondSectionOpened = false;
        thirdSectionOpened = true;
        roach.SetActive(true);

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
        bathroomSFX.PlayCrawlNoises();
        thirdSectionOpened = false;
        fourthSectionOpened = true;
        roach2.SetActive(true);

    }



    private void WaveOne()
    {
        wave1started = true;
        firstWaveOfRoaches.SetActive(true);
        bathroomSFX.WaveNoise();
        fourthSectionOpened = false;

    }

    private void WaveTwo()
    {
        wave1started = false;
        wave2started = true;
        secondWaveOfRoaches.SetActive(true);
        bathroomSFX.WaveNoise();
    }

    public void BackToMainScene()
    {
        if (!horrorGameStarted && canLeaveBathroom)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }
        else
        {
            bathroomSFX.PlayDoorBang();
        }
    }

    private void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

}
