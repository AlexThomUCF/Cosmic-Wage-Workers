using UnityEngine;

public class PlayerReturnToStart : MonoBehaviour
{
    public BathroomsUnhide bathroomUnhideScript;
    public Vector3 startingPosition;
    public GameObject playerCamera;
    public Quaternion startingRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bathroomUnhideScript.playerReturning)
        {
            transform.position = startingPosition; // Change to your desired starting position
            playerCamera.transform.rotation = startingRotation; // Change to your desired starting rotation
            bathroomUnhideScript.playerReturning = false; // Reset the flag after returning to start
        }
    }
}
