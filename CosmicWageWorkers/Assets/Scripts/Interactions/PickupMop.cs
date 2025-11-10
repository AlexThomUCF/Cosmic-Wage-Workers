using UnityEngine;
using UnityEngine.UIElements;

public class PickupMop : MonoBehaviour
{
    [Header("Mop Settings")]
    public Transform handHoldPoint;         // Where the mop is held
    public float pickupRange = 2f;          // How close you must be to pick up
    public LayerMask mopLayer;
    public GameObject cameraOBJ;

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
        RaycastHit hit;
        if (Physics.Raycast(cameraOBJ.transform.position, cameraOBJ.transform.forward, out hit, pickupRange))
        {
            if (hit.transform.CompareTag("Mop"))
            {
                heldMop = hit.transform.gameObject;

                // Disables the Mop's physics while holding
                Rigidbody rb = heldMop.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.isKinematic = true;

                // Disable colliders to prevent collisions while holding
                foreach (Collider c in heldMop.GetComponentsInChildren<Collider>())
                    c.enabled = false;

                // Parents the Mop to the hold point
                heldMop.transform.SetParent(handHoldPoint);
                heldMop.transform.localPosition = Vector3.zero;
                heldMop.transform.localRotation = Quaternion.identity;

                isHoldingMop = true;
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

        // Unparents the mop
        heldMop.transform.SetParent(null);

        // Re-enables physics
        Rigidbody rb = heldMop.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;

        // Re-enables colliders
        foreach (Collider c in heldMop.GetComponentsInChildren<Collider>())
            c.enabled = true;

        // Drops the Mop slightly in front of player
        heldMop.transform.position += cameraOBJ.transform.forward * 0.5f;

        heldMop = null;
        isHoldingMop = false;
    }
}