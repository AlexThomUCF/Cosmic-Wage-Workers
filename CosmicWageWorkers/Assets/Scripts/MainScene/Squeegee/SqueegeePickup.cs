using UnityEngine;

public class SqueegeePickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform holdPoint;
    public GameObject cameraOBJ;
    public float maxPickupRange = 3f;
    public AudioSource Pickupsqgsound;

    [Header("Scale Settings")]
    public Vector3 heldScale = new Vector3(1f, 1f, 1f);
    public Vector3 droppedScale = new Vector3(1f, 1f, 1f);

    private GameObject heldSqueegee;
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
        if (controls.Gameplay.Interact.WasPressedThisFrame())
        {
            if (heldSqueegee != null)
            {
                DropSqueegee();
            }
            else
            {
                TryPickUpSqueegee();
            }
        }

        if (heldSqueegee != null)
        {
            heldSqueegee.transform.position = holdPoint.position;
            heldSqueegee.transform.rotation = holdPoint.rotation;
        }
    }

    private void TryPickUpSqueegee()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraOBJ.transform.position, cameraOBJ.transform.forward, out hit, maxPickupRange, LayerMask.GetMask("Interactable")))
        {
            if (hit.transform.CompareTag("Eraser"))
            {
                PickupSqueegee(hit.transform.gameObject);
            }
        }
    }

    private void PickupSqueegee(GameObject squeegee)
    {
        if (heldSqueegee != null) return;

        heldSqueegee = squeegee;
        heldRb = squeegee.GetComponent<Rigidbody>();
        heldCol = squeegee.GetComponent<Collider>();

        if (heldRb != null) heldRb.isKinematic = true;
        if (heldCol != null) heldCol.enabled = false;

        squeegee.transform.SetParent(holdPoint);
        squeegee.transform.localPosition = Vector3.zero;
        squeegee.transform.localRotation = Quaternion.identity;
        squeegee.transform.localScale = heldScale;

        squeegee.transform.up = Vector3.up;
            Pickupsqgsound.Play();
    }

    public void DropSqueegee()
    {
        if (heldSqueegee == null) return;

        heldSqueegee.transform.SetParent(null);
        heldSqueegee.transform.localScale = droppedScale;

        if (heldRb != null) heldRb.isKinematic = false;
        if (heldCol != null) heldCol.enabled = true;

        heldSqueegee = null;
        heldRb = null;
        heldCol = null;
    }

    public GameObject GetHeldSqueegee() => heldSqueegee;
    public void ForceDropSqueegee() => DropSqueegee();
    public bool IsHoldingSqueegee() => heldSqueegee != null;
}