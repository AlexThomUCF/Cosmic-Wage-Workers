using UnityEngine;

public class TrashPickup : MonoBehaviour
{
    public GameObject cameraOBJ;
    public Transform holdPoint;
    public float maxPickupRange = 3f;

    private GameObject heldTrash;
    private bool isHolding = false;
    private Collider playerCollider;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        playerCollider = cameraOBJ.GetComponentInParent<Collider>();
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

    private void FixedUpdate()
    {
        if (isHolding && heldTrash != null)
        {
            heldTrash.transform.position = holdPoint.position;
            heldTrash.transform.rotation = holdPoint.rotation;
        }
    }

    private void Interact()
    {
        if (!isHolding)
            TryPickUpTrash();
        else
            DropTrash();
    }

    private void TryPickUpTrash()
    {
        RaycastHit hit;

        if (Physics.Raycast(cameraOBJ.transform.position, cameraOBJ.transform.forward, out hit, maxPickupRange))
        {
            if (hit.transform.CompareTag("TrashBag"))
            {
                heldTrash = hit.transform.gameObject;

                heldTrash.transform.SetParent(holdPoint);
                heldTrash.transform.localPosition = Vector3.zero;
                heldTrash.transform.localRotation = Quaternion.identity;

                Rigidbody rb = heldTrash.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                if (playerCollider != null)
                {
                    foreach (Collider c in heldTrash.GetComponentsInChildren<Collider>())
                        Physics.IgnoreCollision(c, playerCollider, true);
                }

                isHolding = true;
            }
        }
    }

    public void ForceDropTrash()
    {
        DropTrash();
    }

    private void DropTrash()
    {
        if (heldTrash == null) return;

        heldTrash.transform.SetParent(null);

        Rigidbody rb = heldTrash.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if (playerCollider != null)
        {
            foreach (Collider c in heldTrash.GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(c, playerCollider, false);
        }

        heldTrash = null;
        isHolding = false;
    }

    public bool IsHoldingTrash()
    {
        return isHolding && heldTrash != null;
    }

    public GameObject GetHeldTrash()
    {
        return heldTrash;
    }
}