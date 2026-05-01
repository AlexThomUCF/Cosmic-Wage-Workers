using UnityEngine;

public class SetActive2 : MonoBehaviour
{
    private GameObject door;
    private BoxCollider doorCollider;
    private GameObject highlight;

    void Awake()
    {
        door = GameObject.Find("Womens Door Trigger");

        doorCollider = door.GetComponent<BoxCollider>();
        highlight = door.transform.Find("Highlight").gameObject;

        doorCollider.enabled = false;
        highlight.SetActive(false);
    }

    public void ActivateObject()
    {
        doorCollider.enabled = true;
        highlight.SetActive(true);
    }
}

