using UnityEngine;

public class BathroomsUnhide : MonoBehaviour
{
    public GameObject player;

    public GameObject firstWall;

    public GameObject firstPuddle;

    public GameObject firstSection;

    public GameObject firstLight;

    public GameObject secondWall;

    public GameObject secondLight;

    public GameObject secondPuddle;

    public GameObject secondSection;

    public GameObject thirdWall;

    public GameObject thirdLight;

    public GameObject thirdPuddle;

    public GameObject thirdSection;

    public GameObject fourthLight;

    public GameObject lastLight;

    public Animator doorAnimator;

    public GameObject lastSoup;

    private bool horrorGameStarted = true; 
    private bool doorOpened = false;

    public float doorTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.z > 341 && horrorGameStarted)
        {
            firstWall.SetActive(false);
            firstPuddle.SetActive(false);
            firstLight.SetActive(false);
            secondLight.SetActive(true);
            firstSection.SetActive(true);
        }

        if (player.transform.position.z > 356 && horrorGameStarted)
        {
            secondWall.SetActive(false);
            secondPuddle.SetActive(false);
            secondLight.SetActive(false);
            thirdLight.SetActive(true);
            secondSection.SetActive(true);
        }
        if (player.transform.position.z > 387 && horrorGameStarted)
        {
            thirdWall.SetActive(false);
            thirdPuddle.SetActive(false);
            thirdLight.SetActive(false);
            fourthLight.SetActive(true);
            thirdSection.SetActive(true);

        }

        if (doorOpened)
        {
            doorTimer -= Time.deltaTime;
            if (doorTimer <= 0)
            {
                CloseDoor();
                doorOpened = false;
                horrorGameStarted = false;
                lastSoup.SetActive(true);
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 332f);
                SectionsTurnedOff();

            }
        }
    }

    public void OpenDoor()
    {
        doorAnimator.SetTrigger("DoorOpen");
        doorOpened = true;
    }   

    public void CloseDoor()
    {
        doorAnimator.SetTrigger("DoorClosed");
    }

    private void SectionsTurnedOff()
    {
        firstSection.SetActive(false);
        secondSection.SetActive(false);
        thirdSection.SetActive(false);
        fourthLight.SetActive(false);
        firstWall.SetActive(true);
        firstLight.SetActive(true);
        lastLight.SetActive(true);

    }

    public void BackToMainScene()
    {
        if (!horrorGameStarted)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }
    }

}
