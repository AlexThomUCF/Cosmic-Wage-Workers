using System.Collections;
using UnityEngine;

public class OneWayTeleporter : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource Teleportsfx;
    [Header("Assign these")]
    public Transform entryPortal;   // trigger is here (or on this GameObject)
    public Transform exitPortal;    // destination

    [Header("Player detection")]
    public string playerTag = "Player";
    public string mainPlayerName = "MainPlayer"; // object to move (optional)

    [Header("Teleport behavior")]
    public float cooldown = 0.25f;

    [Tooltip("Move a little forward after exit so you're not inside stuff.")]
    public float exitForwardOffset = 1.0f;

    [Tooltip("Lift a bit to avoid spawning in ground.")]
    public float exitUpOffset = 0.3f;

    [Header("Ground snap (recommended for uneven tracks)")]
    public bool snapToGround = true;
    public float groundRayUp = 10f;
    public float groundRayDown = 60f;

    [Header("Vehicle safety")]
    public bool zeroRigidbodyVelocity = true;

    private bool canTeleport = true;

    void Reset()
    {
        entryPortal = transform;
    }

    void Awake()
    {
        if (entryPortal == null) entryPortal = transform;

        // Ensure the entry has a trigger collider
        var col = entryPortal.GetComponent<Collider>();
        if (col == null) col = entryPortal.gameObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canTeleport) return;
        if (other == null) return;
        if (!other.CompareTag(playerTag)) return;
        if (exitPortal == null) return;

        // Decide what object to move:
        // Prefer the named MainPlayer if it exists, otherwise move the collider's root.
        GameObject target = GameObject.Find(mainPlayerName);
        if (target == null) target = other.transform.root.gameObject;

        Teleport(target);
        Teleportsfx.Play();
    }

    private void Teleport(GameObject target)
    {
        Vector3 targetPos = exitPortal.position;

        // Flatten forward so slopes don't push you down into the track
        Vector3 flatForward = Vector3.ProjectOnPlane(exitPortal.forward, Vector3.up).normalized;
        if (flatForward.sqrMagnitude < 0.001f) flatForward = Vector3.forward;

        if (snapToGround)
        {
            Vector3 rayStart = targetPos + Vector3.up * groundRayUp;
            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, groundRayUp + groundRayDown))
                targetPos = hit.point + Vector3.up * exitUpOffset;
            else
                targetPos += Vector3.up * exitUpOffset;
        }
        else
        {
            targetPos += Vector3.up * exitUpOffset;
        }

        targetPos += flatForward * exitForwardOffset;

        target.transform.position = targetPos;
        target.transform.rotation = Quaternion.LookRotation(flatForward, Vector3.up);

        if (zeroRigidbodyVelocity)
        {
            var rb = target.GetComponent<Rigidbody>();
            if (rb == null) rb = target.GetComponentInChildren<Rigidbody>();

            if (rb != null)
            {
#if UNITY_6000_0_OR_NEWER
                rb.linearVelocity = Vector3.zero;
#else
                rb.velocity = Vector3.zero;
#endif
                rb.angularVelocity = Vector3.zero;
            }
        }

        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        canTeleport = false;
        yield return new WaitForSeconds(cooldown);
        canTeleport = true;
    }
}