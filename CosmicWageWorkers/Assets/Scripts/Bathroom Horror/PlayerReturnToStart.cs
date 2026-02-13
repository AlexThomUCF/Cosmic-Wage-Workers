using UnityEngine;

public class PlayerReturnToStart : MonoBehaviour
{
    public BathroomsUnhide bathroomUnhideScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bathroomUnhideScript.playerReturning)
        {
            transform.position = new Vector3(-111f, 5f, 325f); // Change to your desired starting position
            bathroomUnhideScript.playerReturning = false; // Reset the flag after returning to start
        }
    }
}
