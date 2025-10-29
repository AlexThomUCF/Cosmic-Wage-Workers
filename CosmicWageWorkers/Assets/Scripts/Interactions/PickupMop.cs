using UnityEngine;

public class PlayerMopHandler : MonoBehaviour
{
    [Header("Mop Settings")]
    public Transform handHoldPoint;         // Where the mop is held
    public float pickupRange = 2f;          // How close you must be to pick up
    public LayerMask mopLayer;

    private GameObject heldMop;
    private bool isHoldingMop;

    public bool IsHoldingMop() => isHoldingMop;

    [Header("Optional Event")]
    public UnityEngine.Events.UnityEvent onInteract;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.Interact.performed += _ => Interact();
    }

    private void OnDisable()
    {
        controls.Gameplay.Interact.performed -= _ => Interact();
        controls.Gameplay.Disable();
    }

    private void Interact()
    {
        onInteract?.Invoke();

        if (!isHoldingMop)
            TryPickUpMop();
        else
            DropMop();
    }

    private void TryPickUpMop()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange, mopLayer);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Mop"))
            {
                heldMop = hit.gameObject;

                // Disable physics while held
                if (heldMop.TryGetComponent<Rigidbody>(out var rb))
                    rb.isKinematic = true;

                // Parent to hand point
                heldMop.transform.SetParent(handHoldPoint);
                heldMop.transform.localPosition = Vector3.zero;
                heldMop.transform.localRotation = Quaternion.identity;

                isHoldingMop = true;
                return;
            }
        }
    }

    public void ForceDropMop()
    {
        if (isHoldingMop)
            DropMop();
    }

    private void DropMop()
    {
        if (heldMop == null) return;

        // Unparent
        heldMop.transform.SetParent(null);

        // Re-enable physics
        if (heldMop.TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = false;

        // Small forward toss
        heldMop.transform.position += transform.forward * 0.5f;

        heldMop = null;
        isHoldingMop = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}