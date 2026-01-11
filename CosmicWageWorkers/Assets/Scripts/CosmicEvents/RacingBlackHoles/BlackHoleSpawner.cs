using System.Collections;
using UnityEngine;

public class BlackHolesSceneStarter : MonoBehaviour
{
    [Header("Reference")]
    public BlackHoles blackHoles;

    [Header("Portal Find Settings")]
    public string blackHoleBaseName = "Black Hole VFX"; // matches "Black Hole VFX(Clone)"

    [Header("Spawn Settings")]
    public bool spawnOnStart = true;

    [Tooltip("How long after entering a portal before respawning the pair (lets teleport finish first).")]
    public float respawnDelay = 0.05f;

    [Tooltip("Prevents double triggers if the player overlaps triggers for a moment.")]
    public float respawnLockout = 0.5f;

    private bool respawnLocked = false;

    void Start()
    {
        if (spawnOnStart)
            RespawnBlackHoles();
    }

    /// <summary>
    /// Destroys old portals, cancels old coroutines, spawns fresh pair, and attaches use listeners.
    /// Safe to call multiple times.
    /// </summary>
    public void RespawnBlackHoles()
    {
        if (blackHoles == null)
        {
            Debug.LogWarning("[BlackHolesSceneStarter] BlackHoles reference not set!");
            return;
        }

        // Stop old lifetime/cooldown coroutines so old timers don’t delete the new pair later
        blackHoles.StopAllCoroutines();

        // Destroy any existing portal clones
        DestroyExistingBlackHoles();

        // Spawn a fresh linked pair
        blackHoles.TriggerBlackHoles();

        // Attach listeners after the clones exist
        StartCoroutine(AttachListenersNextFrame());
    }

    private IEnumerator AttachListenersNextFrame()
    {
        // wait a frame so Instantiate() has happened
        yield return null;

#if UNITY_2023_1_OR_NEWER
        var transforms = Object.FindObjectsByType<Transform>(FindObjectsSortMode.None);
#else
        var transforms = Object.FindObjectsOfType<Transform>();
#endif

        int attached = 0;

        // Attach to ALL portal clones and, crucially, to the object(s) that have trigger colliders
        foreach (var t in transforms)
        {
            if (t == null) continue;
            var root = t.gameObject;
            if (root == null || root.name == null) continue;

            if (!root.name.StartsWith(blackHoleBaseName)) continue;

            attached += AddUseListenerToPortalColliders(root);
        }

        if (attached == 0)
        {
            Debug.LogWarning("[BlackHolesSceneStarter] No listeners attached. " +
                             "Likely the portal name doesn't match blackHoleBaseName or there are no colliders under the clone.");
        }
    }

    /// <summary>
    /// Adds a listener to the GameObject(s) that actually receive trigger events (those with Collider components).
    /// Returns how many listeners were attached.
    /// </summary>
    private int AddUseListenerToPortalColliders(GameObject portalRoot)
    {
        int count = 0;

        // Get all colliders in this portal clone hierarchy (includes root + children)
        Collider[] cols = portalRoot.GetComponentsInChildren<Collider>(true);

        // If there are no colliders, fall back to adding on root
        if (cols == null || cols.Length == 0)
        {
            AddUseListener(portalRoot);
            return 1;
        }

        foreach (var col in cols)
        {
            if (col == null) continue;

            // We only care about trigger colliders (that’s what fires OnTriggerEnter)
            // BUT: if the BlackHoles script creates the trigger at runtime, it’s on the root and will be trigger anyway.
            if (!col.isTrigger) continue;

            AddUseListener(col.gameObject);
            count++;
        }

        // If no trigger colliders were found (some setups), attach to root as a fallback
        if (count == 0)
        {
            AddUseListener(portalRoot);
            count = 1;
        }

        return count;
    }

    private void AddUseListener(GameObject obj)
    {
        var listener = obj.GetComponent<PortalUseListener>();
        if (listener == null) listener = obj.AddComponent<PortalUseListener>();

        listener.owner = this;
    }

    private void DestroyExistingBlackHoles()
    {
#if UNITY_2023_1_OR_NEWER
        var transforms = Object.FindObjectsByType<Transform>(FindObjectsSortMode.None);
#else
        var transforms = Object.FindObjectsOfType<Transform>();
#endif
        foreach (var t in transforms)
        {
            if (t == null) continue;

            GameObject go = t.gameObject;
            if (go != null && go.name != null && go.name.StartsWith(blackHoleBaseName))
                Destroy(go);
        }
    }

    // Called by PortalUseListener when either portal is entered
    public void NotifyPortalUsed()
    {
        if (respawnLocked) return;
        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        respawnLocked = true;

        // let the teleport happen first
        if (respawnDelay > 0f)
            yield return new WaitForSeconds(respawnDelay);

        RespawnBlackHoles();

        // lockout so we don't instantly retrigger while overlapping
        if (respawnLockout > 0f)
            yield return new WaitForSeconds(respawnLockout);

        respawnLocked = false;
    }
}

/// <summary>
/// Added at runtime. Detects player entry and tells the scene starter.
/// Must be on the same GameObject that has the TRIGGER collider.
/// </summary>
public class PortalUseListener : MonoBehaviour
{
    [HideInInspector] public BlackHolesSceneStarter owner;

    private void OnTriggerEnter(Collider other)
    {
        if (owner == null) return;

        if (other != null && other.CompareTag("Player"))
            owner.NotifyPortalUsed();
    }
}
