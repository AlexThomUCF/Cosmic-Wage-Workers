using UnityEngine;
using UnityEngine.Events;

public class BoxPickUp : MonoBehaviour
{
    [Header("Pick Up Settings")]
    public float maxPickupRange = 3f;
    public GameObject cameraOBJ; // References to the player camera
    public Transform holdPoint; // Where the box is held

    private GameObject heldBox;
    private bool isHolding = false;

    [Header("Optional Event")]
    public UnityEvent onInteract;

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

        if (!isHolding)
        {
            TryPickUpBox();
        }
        else
        {
            DropBox();
        }
    }

    private void TryPickUpBox()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraOBJ.transform.position, cameraOBJ.transform.forward, out hit, maxPickupRange))
        {
            if (hit.transform.CompareTag("Box"))
            {
                heldBox = hit.transform.gameObject;

                // Disables the box's physics while holding
                Rigidbody rb = heldBox.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.isKinematic = true;

                // Disable colliders to prevent collisions while holding
                foreach (Collider c in heldBox.GetComponentsInChildren<Collider>())
                    c.enabled = false;

                // Parents the box to the hold point
                heldBox.transform.SetParent(holdPoint);
                heldBox.transform.localPosition = Vector3.zero;
                heldBox.transform.localRotation = Quaternion.identity;

                isHolding = true;
            }
        }
    }

    public bool IsHoldingBox()
    {
        return isHolding && heldBox != null;
    }

    public void ForceDropBox()
    {
        if (isHolding)
            DropBox();
    }

    private void DropBox()
    {
        if (heldBox == null) return;

        // Unparents the box
        heldBox.transform.SetParent(null);

        // Re-enables physics
        Rigidbody rb = heldBox.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;

        // Re-enables colliders
        foreach (Collider c in heldBox.GetComponentsInChildren<Collider>())
            c.enabled = true;

        // Drops the box slightly in front of player
        heldBox.transform.position += cameraOBJ.transform.forward * 0.5f;

        heldBox = null;
        isHolding = false;
    }
}

