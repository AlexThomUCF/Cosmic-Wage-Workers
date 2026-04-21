using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class BathroomsUnhide : MonoBehaviour
{
    public GameObject player;
    public GameObject mainCamera;
    public GameObject firstPersonCamera;
    public GameObject cineMachine;



    

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
    public GameObject roachJS;
    public Animator doorAnimator;
    public Animator roachAnimator;
    public Animator cameraAnimator;
    public Animator bsAnimator;
    public BathroomSFX bathroomSFX;
    public GameObject blackScreen;
    public GameObject jumpScareBlackScreen;
    public GameObject promptUpUI;
    public GameObject speechBubble;
    public GameObject alternatePromptUI;
   




    [Header("*** Variables ***")]
    public bool horrorGameStarted = true; 
    public bool doorOpened = false;

    [Header("Customer Interaction ID")]
    public string interactionID;


    private bool firstSectionOpened = true;
    private bool secondSectionOpened = false;
    private bool thirdSectionOpened = false;
    private bool fourthSectionOpened = false;
    private bool canLeaveBathroom = false;
    private bool wave1started = false;
    private bool wave2started = false;
    private bool restartingLevel = false;
    private bool lightsOn = true;
    private bool cineMachineActivated = false;
    private bool cineMachineDeactivated = false;
    private bool doorPrompt = false;
    private bool doorSpeechBubble = false;

    public float doorTimer;
    public float roachTimer;
    public float lightTimer;
    
    public float blackScreenTimer;
    public bool playerReturning = false;
    

    public float doorCloseDelay;
    public float cinemachineDelay = 3.5f;
    public float blackScreenDelay = 1.65f; 
    public Roach roachScript;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cineMachineActivated = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cineMachineActivated)
        {
            cinemachineDelay -= Time.deltaTime;
            if (cinemachineDelay <= 0)
            {
                cineMachine.SetActive(false);
                cineMachineActivated = false;
                jumpScareBlackScreen.SetActive(true);
                bathroomSFX.distortedStoreMusic.Play();
                player.SetActive(true);
                cinemachineDelay = 3.5f;
                cineMachineDeactivated = true;
            }
        }
        if (cineMachineDeactivated)
        {
            blackScreenDelay -= Time.deltaTime;
            if (blackScreenDelay <= 0)
            {
                jumpScareBlackScreen.SetActive(false);
                cineMachineDeactivated = false;
                blackScreenDelay = 1.65f;
                bsAnimator.SetTrigger("BSFade");
            }
        }

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

        if (player.transform.position.z > 422 && wave1started && !wave2started)
        {
            WaveTwo();
        }

        if (player.transform.position.z >= 430 && !doorPrompt)
        {
            alternatePromptUI.SetActive(true);
            doorPrompt = true;
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

        if (roachScript.roachExpansion1Active && player.transform.position.z < 423)
        {
            RestartLevel();
        }

        if (roachScript.roachExpansion2Active && player.transform.position.z < 432)
        {
            RestartLevel();
        }

        if (restartingLevel)
        {
            blackScreenTimer -= Time.deltaTime;
            if (blackScreenTimer <= 0)
            {
                jumpScareBlackScreen.SetActive(false);
                JumpScare();
                roachTimer -= Time.deltaTime;
            }
             if (roachTimer <= 0)
            {
                roachJS.SetActive(false);
                roachTimer = 1.2f;
                restartingLevel = false;
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }

        }





        if (doorOpened)
        {
            doorTimer -= Time.deltaTime;
            if (doorTimer <= 1.5f && !doorSpeechBubble)
            {
                speechBubble.SetActive(true);
                bathroomSFX.bathSource.PlayOneShot(bathroomSFX.getOut);
                doorSpeechBubble = true;
            }
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
        StartCoroutine(OpenEventDoorRoutine());
    }

    private IEnumerator OpenEventDoorRoutine()
    {   
        bathroomSFX.bathSource.PlayOneShot(bathroomSFX.knock);
        alternatePromptUI.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        bathroomSFX.StopAllMusic();
        bathroomSFX.bathSource.PlayOneShot(bathroomSFX.doorOpen);
        
        roachScript.StopRoachSpawning();
        RoachesDisappear();
        bsAnimator.SetTrigger("BSFastFade");
        doorAnimator.SetTrigger("DoorOpen");
        doorOpened = true;
        promptUpUI.SetActive(false);
    }   

    public void CloseEventDoor()
    {
        bathroomSFX.bathSource.PlayOneShot(bathroomSFX.doorClose);
        doorAnimator.SetTrigger("DoorClosed");
        blockOut.SetActive(false);      
    }


    private void ReturnToBathroom()
    {
        bsAnimator.SetTrigger("BSFade");
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
        doorOpened = false;
        playerReturning = true;
        horrorGameStarted = false;
        lastSoup.SetActive(true);
        lastMop.SetActive(true);
        speechBubble.SetActive(false);
        doorPrompt = false;
        doorSpeechBubble = false;
        bathroomSFX.lightNoise.Play();

    }

    private void FirstSection()
    {
        bsAnimator.SetTrigger("BSFastFade");
        firstWall.SetActive(false);
        firstPuddle.SetActive(false);
        firstLight.SetActive(false);
        secondLight.SetActive(true);
        firstSection.SetActive(true);
        firstMop.SetActive(false);
        bathroomSFX.distortedStoreMusic.Stop();
        bathroomSFX.bathSource.PlayOneShot(bathroomSFX.sectionOpen);
        bathroomSFX.backgroundNoise.Play();
        firstSectionOpened = false;
        secondSectionOpened = true;
        blockOut.SetActive(true);

    }

    private void SecondSection()
    {
        bsAnimator.SetTrigger("BSFastFade");
        secondWall.SetActive(false);
        secondPuddle.SetActive(false);
        secondLight.SetActive(false);
        thirdLight.SetActive(true);
        secondSection.SetActive(true);
        secondMop.SetActive(false);
        bathroomSFX.bathSource.PlayOneShot(bathroomSFX.sectionOpen);
        bathroomSFX.bugNoises.Play();
        secondSectionOpened = false;
        thirdSectionOpened = true;
        roach.SetActive(true);

    }

    private void ThirdSection()
    {
        bsAnimator.SetTrigger("BSFastFade");
        thirdWall.SetActive(false);
        thirdPuddle.SetActive(false);
        thirdLight.SetActive(false);
        fourthLight.SetActive(true);
        thirdSection.SetActive(true);
        thirdMop.SetActive(false); 
        bathroomSFX.bathSource.PlayOneShot(bathroomSFX.sectionOpen);
        bathroomSFX.crawlNoise.Play();
        thirdSectionOpened = false;
        fourthSectionOpened = true;
        roach2.SetActive(true);
    }



    private void WaveOne()
    {
        bsAnimator.SetTrigger("BSFastFade");
        bathroomSFX.heartBeat.Play();
        wave1started = true;
        firstWaveOfRoaches.SetActive(true);
        bathroomSFX.bathSource.PlayOneShot(bathroomSFX.waveStarted);
        fourthSectionOpened = false;

    }

    private void WaveTwo()
    {
        bsAnimator.SetTrigger("BSFastFade");
        wave1started = false;
        wave2started = true;
        secondWaveOfRoaches.SetActive(true);
        bathroomSFX.bathSource.PlayOneShot(bathroomSFX.waveStarted);
    }

    public void RoachExpansionOne()
    {
        bsAnimator.SetTrigger("BSFastFade");
        bathroomSFX.heartBeat.PlayOneShot(bathroomSFX.heavyBreathing);
        roachScript.roachSpawningActive = false;
    }

    public void RoachExpansionTwo()
    {
        bsAnimator.SetTrigger("BSFastFade");
        roachScript.roachExpansion1Active = false;
    }

    public void BackToMainScene()
    {
        if (!horrorGameStarted && canLeaveBathroom)
        {
            if(!string.IsNullOrEmpty(interactionID))
            {
                CustomerManager.MarkInteractionComplete(interactionID);
            }
            SaveSystem.SaveGame();
            FinalMiniGame.miniGameCount++;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }
        else
        {
            bathroomSFX.bathSource.PlayOneShot(bathroomSFX.doorBang);
        }
    }

    public void RestartLevel()
    {
        blackScreen.SetActive(false);
        jumpScareBlackScreen.SetActive(true);
        restartingLevel = true;
        roach.SetActive(false);
        roachJS.SetActive(true);
        mainCamera.SetActive(false);
        firstPersonCamera.SetActive(false);
        roachJS.transform.position = player.transform.position;
        roachJS.transform.rotation = player.transform.rotation;
        roach2.SetActive(false);
        firstWaveOfRoaches.SetActive(false);
        bathroomSFX.StopAllMusic();
        secondWaveOfRoaches.SetActive(false);
    }

    private void RoachesDisappear()
    {
        roach.SetActive(false);
        roach2.SetActive(false);
        firstWaveOfRoaches.SetActive(false);
        secondWaveOfRoaches.SetActive(false);
    }
    private void JumpScare()
    {
        blackScreen.SetActive(false);
        bathroomSFX.jumpScare.PlayOneShot(bathroomSFX.jumpScareSound);
        cameraAnimator.SetTrigger("JumpScare");
        roachAnimator.SetTrigger("RoachJS");
    }

}
