using UnityEngine;

public class TrashPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform holdPoint;
    public GameObject cameraOBJ;
    public float maxPickupRange = 3f;
    public AudioSource Pickuptrashsound;

    [Header("Scale Settings")]
    public Vector3 heldScale = new Vector3(0.4f, 0.4f, 0.4f);
    public Vector3 droppedScale = new Vector3(1f, 1f, 1f);

    private GameObject heldTrash;
    private Rigidbody heldRb;
    private Collider heldCol;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable() => controls.Gameplay.Enable();
    private void OnDisable() => controls.Gameplay.Disable();

    private void Update()
    {
        // Interact button
        if (controls.Gameplay.Interact.WasPressedThisFrame())
        {
            if (heldTrash != null)
            {
                DropTrash();
            }
            else
            {
                TryPickUpTrash();
            }
        }

        // Keep the held trash at the hold point
        if (heldTrash != null)
        {
            heldTrash.transform.position = holdPoint.position;
            heldTrash.transform.rotation = holdPoint.rotation;
        }
    }

    private void TryPickUpTrash()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraOBJ.transform.position, cameraOBJ.transform.forward, out hit, maxPickupRange, LayerMask.GetMask("Interactable")))
        {
            if (hit.transform.CompareTag("TrashBag"))
            {
                PickupTrash(hit.transform.gameObject);
            }
        }
    }

private void PickupTrash(GameObject trash)
{
    if (heldTrash != null) return;

    heldTrash = trash;

    // Hide all children
    foreach (Transform child in trash.transform)
        {
            child.gameObject.SetActive(false);
        }
    heldRb = trash.GetComponent<Rigidbody>();
    heldCol = trash.GetComponent<Collider>();

    if (heldRb != null) heldRb.isKinematic = true;
    if (heldCol != null) heldCol.enabled = false;

    trash.transform.SetParent(holdPoint);
    trash.transform.localPosition = Vector3.zero;
    trash.transform.localRotation = Quaternion.identity;
    trash.transform.localScale = heldScale;

    // Ensure the trash bag is upright
    trash.transform.up = Vector3.up;
        Pickuptrashsound.Play();
}

    public void DropTrash()
    {
        if (heldTrash == null) return;

        heldTrash.transform.SetParent(null);
        heldTrash.transform.localScale = droppedScale;

        if (heldRb != null) heldRb.isKinematic = false;
        if (heldCol != null) heldCol.enabled = true;

        heldTrash = null;
        heldRb = null;
        heldCol = null;
    }

    // Functions for TrashDisposal or other scripts
    public GameObject GetHeldTrash() => heldTrash;
    public void ForceDropTrash() => DropTrash();
    public bool IsHoldingTrash() => heldTrash != null;
}