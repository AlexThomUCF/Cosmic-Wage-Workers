using UnityEngine;

public class Roach : MonoBehaviour
{
    private float roachSpawning = 6f;
    private float countdownTimer = 3f; 
    
    public bool roachSpawningActive = true;
    public bool roachExpansion1Active = false;
    public bool roachExpansion2Active = false;
    public GameObject roachExpansion1;
    public GameObject roachExpansion2;

    public BathroomsUnhide bathroomsUnhide;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        roachSpawning -= Time.deltaTime;
        if (roachSpawning <= 3f && roachSpawningActive)
        {
            roachExpansion1.SetActive(true);
            roachExpansion1Active = true;
            bathroomsUnhide.RoachExpansionOne();

           
        }
        if (roachSpawning <= 0f && roachExpansion1Active)
        {
            roachExpansion2.SetActive(true);
            roachExpansion2Active = true;
            bathroomsUnhide.RoachExpansionTwo();
        }

        if (roachSpawning <= 0f && roachExpansion2Active) 
        {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0f)
            {
                bathroomsUnhide.RestartLevel();
            }
        }

    }

    public void StopRoachSpawning()
    {
        roachSpawningActive = false;
        roachExpansion1Active = false;
        roachExpansion2Active = false; 
    }
}
