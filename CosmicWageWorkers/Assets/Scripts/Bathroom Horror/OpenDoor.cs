using System.Collections;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public float rotationAngle = 90f;
    public float rotationSpeed = 3.5f;
    public bool rotateOnZ = true;
    public BathroomSFX bathroomSFX;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, rotationAngle));
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void ToggleDoor()
    {
        StartCoroutine(RotateDoor());
        if (isOpen)
        {
            bathroomSFX.PlayDoorClose();
        }
        else
        {
            bathroomSFX.PlayDoorOpen();
        }
    }

    private IEnumerator RotateDoor()
    {
        Quaternion targetRotation = isOpen ? closedRotation : openRotation;
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }
        transform.rotation = targetRotation;
        isOpen = !isOpen;
    }
}
