using UnityEngine;

public class SetActive2 : MonoBehaviour
{
   
    public GameObject door;
    private BoxCollider doorCollider;

    void Awake()
    {
        GameObject door = GameObject.Find("Womens Door Trigger");
        doorCollider = door.GetComponent<BoxCollider>();

        doorCollider.enabled = false; // disable collider
    }

    public void ActivateObject()
    {
        doorCollider.enabled = true; // enable collider
    }


}

