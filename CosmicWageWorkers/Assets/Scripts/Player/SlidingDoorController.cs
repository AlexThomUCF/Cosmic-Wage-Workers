using UnityEngine;

public class SlidingDoorController : MonoBehaviour
{
    [Header("Door References")]
    public Transform leftGlassDoor;
    public Transform leftSlidingDoor;
    public Transform rightGlassDoor;
    public Transform rightSlidingDoor;

    [Header("Door Settings")]
    public float closedXPosition = -0.09f;
    public float openXPositionLeft = 7f;
    public float closedXPositionRight = 0.052f;
    public float openXPositionRight = -7f;
    public float doorSpeed = 5f;

    [Header("Trigger Settings")]
    public float triggerRadius = 5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;

    private bool isOpen = false;
    private int entitiesNearby = 0;

    void Start()
    {
        BoxCollider trigger = gameObject.AddComponent<BoxCollider>();
        trigger.isTrigger = true;
        trigger.size = new Vector3(triggerRadius * 2, 5f, triggerRadius * 2);

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
        }
    }

    void Update()
    {
        if (entitiesNearby > 0 && !isOpen)
        {
            isOpen = true;

            if (audioSource != null && doorOpenSound != null)
            {
                audioSource.PlayOneShot(doorOpenSound);
            }
        }
        else if (entitiesNearby == 0 && isOpen)
        {
            isOpen = false;

            if (audioSource != null && doorCloseSound != null)
            {
                audioSource.PlayOneShot(doorCloseSound);
            }
        }
    }

    void LateUpdate()
    {
        float targetLeftX = isOpen ? openXPositionLeft : closedXPosition;
        float targetRightX = isOpen ? openXPositionRight : closedXPositionRight;

        if (leftGlassDoor != null)
        {
            leftGlassDoor.localPosition = new Vector3(
                Mathf.MoveTowards(leftGlassDoor.localPosition.x, targetLeftX, doorSpeed * Time.deltaTime),
                leftGlassDoor.localPosition.y,
                leftGlassDoor.localPosition.z
            );
        }

        if (leftSlidingDoor != null)
        {
            leftSlidingDoor.localPosition = new Vector3(
                Mathf.MoveTowards(leftSlidingDoor.localPosition.x, targetLeftX, doorSpeed * Time.deltaTime),
                leftSlidingDoor.localPosition.y,
                leftSlidingDoor.localPosition.z
            );
        }

        if (rightGlassDoor != null)
        {
            rightGlassDoor.localPosition = new Vector3(
                Mathf.MoveTowards(rightGlassDoor.localPosition.x, targetRightX, doorSpeed * Time.deltaTime),
                rightGlassDoor.localPosition.y,
                rightGlassDoor.localPosition.z
            );
        }

        if (rightSlidingDoor != null)
        {
            rightSlidingDoor.localPosition = new Vector3(
                Mathf.MoveTowards(rightSlidingDoor.localPosition.x, targetRightX, doorSpeed * Time.deltaTime),
                rightSlidingDoor.localPosition.y,
                rightSlidingDoor.localPosition.z
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("NormalCustomer"))
        {
            entitiesNearby++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("NormalCustomer"))
        {
            entitiesNearby--;
        }
    }
}