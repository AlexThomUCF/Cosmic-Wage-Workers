using UnityEngine;

public class BathroomsUnhide : MonoBehaviour
{
    public GameObject player;

    public GameObject firstWall;

    public GameObject firstPuddle;

    public GameObject firstSection;

    public GameObject secondWall;

    public GameObject secondPuddle;

    public GameObject secondSection;

    public GameObject thirdWall;

    public GameObject thirdPuddle;

    public GameObject thirdSection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.z > 341)
        {
            firstWall.SetActive(false);
            firstPuddle.SetActive(false);
            firstSection.SetActive(true);
        }

        if (player.transform.position.z > 356)
        {
            secondWall.SetActive(false);
            secondPuddle.SetActive(false);
            secondSection.SetActive(true);
        }
        if (player.transform.position.z > 387)
        {
            thirdWall.SetActive(false);
            thirdPuddle.SetActive(false);
            thirdSection.SetActive(true);
        }
    }
    
    
}
