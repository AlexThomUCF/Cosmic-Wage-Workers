using UnityEngine;
using UnityEngine.UI;

public class BoxPickUp : MonoBehaviour
{
    [Header("Pick Up Settings")]
    public GameObject cameraOBJ;
    public Transform holdPoint;
    public float maxPickupRange = 3f;

    private GameObject heldBox;
    private bool isHolding = false;
    private Collider playerCollider;

    private PlayerControls controls;

    [Header("Pickup Indicator")]
    public Image pickupIndicator;       // UI icon in center
    public float indicatorScaleSpeed = 5f;
    public float maxIndicatorScale = 1f;   // fully visible
    public float minIndicatorScale = 0f;   // hidden

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
        if (isHolding && heldBox != null)
        {
            // Snap box to hold point
            heldBox.transform.position = holdPoint.position;
            heldBox.transform.rotation = holdPoint.rotation;
        }
    }

    private void Update()
    {
        // Smoothly grow/shrink pickup indicator
        if (pickupIndicator != null)
        {
            float targetScale = IsLookingAtBox() ? maxIndicatorScale : minIndicatorScale;
            float newScale = Mathf.Lerp(pickupIndicator.transform.localScale.x, targetScale, Time.unscaledDeltaTime * indicatorScaleSpeed);
            pickupIndicator.transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }

    private bool IsLookingAtBox()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraOBJ.transform.position, cameraOBJ.transform.forward, out hit, maxPickupRange))
        {
            if (hit.transform.CompareTag("Box"))
                return true;
        }
        return false;
    }

    private void Interact()
    {
        if (!isHolding)
            TryPickUpBox();
        else
            DropBox();
    }

    private void TryPickUpBox()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraOBJ.transform.position, cameraOBJ.transform.forward, out hit, maxPickupRange))
        {
            if (hit.transform.CompareTag("Box"))
            {
                heldBox = hit.transform.gameObject;

                // Set fixed scale while holding
                heldBox.transform.localScale = new Vector3(8f, 8f, 8f);

                // Parent to hold point
                heldBox.transform.SetParent(holdPoint);
                heldBox.transform.localPosition = Vector3.zero;
                heldBox.transform.localRotation = Quaternion.identity;

                // Disable physics while held
                Rigidbody rb = heldBox.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                // Ignore collisions with player
                if (playerCollider != null)
                {
                    foreach (Collider c in heldBox.GetComponentsInChildren<Collider>())
                        Physics.IgnoreCollision(c, playerCollider, true);
                }

                isHolding = true;
            }
        }
    }

    public void ForceDropBox()
    {
        DropBox();
    }

    private void DropBox()
    {
        if (heldBox == null) return;

        // Restore fixed scale when dropped
        heldBox.transform.localScale = new Vector3(22f, 22f, 22f);

        // Unparent
        heldBox.transform.SetParent(null);

        // Restore physics
        Rigidbody rb = heldBox.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // Restore collisions
        if (playerCollider != null)
        {
            foreach (Collider c in heldBox.GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(c, playerCollider, false);
        }

        heldBox = null;
        isHolding = false;
    }

    public bool IsHoldingBox() => isHolding && heldBox != null;
}