using UnityEngine;

public class BreakRoomDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public float closedYOffset = 0f;
    public float openYOffset = 5.75f;
    public float doorSpeed = 5f;

    [Header("Proximity Settings")]
    public float openDistance = 5f;
    public float closeDistance = 3f;

    private Transform player;
    private bool isDoorOpen = false;
    private float targetYPosition;
    private float initialYPosition;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        initialYPosition = transform.localPosition.y;
        targetYPosition = initialYPosition + closedYOffset;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isDoorOpen && distanceToPlayer < openDistance)
        {
            isDoorOpen = true;
            targetYPosition = initialYPosition + openYOffset;
        }
        else if (isDoorOpen && distanceToPlayer > openDistance + 2f)
        {
            isDoorOpen = false;
            targetYPosition = initialYPosition + closedYOffset;
        }

        transform.localPosition = new Vector3(
            transform.localPosition.x,
            Mathf.MoveTowards(transform.localPosition.y, targetYPosition, doorSpeed * Time.deltaTime),
            transform.localPosition.z
        );
    }
}
