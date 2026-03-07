using UnityEngine;

public class BreakRoomDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public float closedYOffset = 0f;
    public float openYOffset = 5.75f;
    public float doorSpeed = 5f;

    [Header("Trigger Settings")]
    public float triggerRadius = 5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;

    private bool isDoorOpen = false;
    private float targetYPosition;
    private float initialYPosition;
    private int playersNearby = 0;

    void Start()
    {
        initialYPosition = transform.position.y;
        targetYPosition = initialYPosition + closedYOffset;

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
        if (playersNearby > 0 && !isDoorOpen)
        {
            isDoorOpen = true;
            targetYPosition = initialYPosition + openYOffset;

            if (audioSource != null && doorOpenSound != null)
            {
                audioSource.PlayOneShot(doorOpenSound);
            }
        }
        else if (playersNearby == 0 && isDoorOpen)
        {
            isDoorOpen = false;
            targetYPosition = initialYPosition + closedYOffset;

            if (audioSource != null && doorCloseSound != null)
            {
                audioSource.PlayOneShot(doorCloseSound);
            }
        }
    }

    void LateUpdate()
    {
        transform.position = new Vector3(
            transform.position.x,
            Mathf.MoveTowards(transform.position.y, targetYPosition, doorSpeed * Time.deltaTime),
            transform.position.z
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersNearby++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersNearby--;
        }
    }
}
