using UnityEngine;

public class SpawnStopTrigger : MonoBehaviour
{
    [Header("References")]
    public FallingItemManager itemManager;
    private BoxCollider box;               // trigger volume

    [Header("Detection")]
    public LayerMask playerLayer;

    void Awake()
    {
        box = GetComponent<BoxCollider>();
        if (box == null)
        {
            Debug.LogError("SpawnStopTrigger requires a BoxCollider on the same object.");
        }
    }

    void Update()
    {
        if (box == null || itemManager == null) return;

        Vector3 center = box.bounds.center;
        Vector3 halfExtents = box.bounds.extents;

        // If player is inside the box, stop spawning
        if (Physics.CheckBox(center, halfExtents, transform.rotation, playerLayer))
        {
            itemManager.StopSpawning();
            enabled = false;
        }
    }
}