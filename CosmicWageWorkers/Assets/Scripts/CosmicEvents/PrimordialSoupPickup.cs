using UnityEngine;

public class PrimordialSoupPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform holdPoint;
    public GameObject playerCamera;
    public float maxPickupRange = 3f;

    [Header("Scale Settings")]
    public Vector3 heldScale = new Vector3(0.4f, 0.4f, 0.4f);
    public Vector3 droppedScale = new Vector3(1f, 1f, 1f);

    private GameObject heldSoup;
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
        // Pickup or drop
        if (controls.Gameplay.Interact.WasPressedThisFrame())
        {
            if (heldSoup != null)
            {
                DropSoup();
            }
            else
            {
                TryPickUpSoup();
            }
        }

        // Consume soup while held
        if (heldSoup != null && controls.Gameplay.Use.WasPressedThisFrame())
        {
            ConsumeSoup();
        }

        // Keep held soup at hold point
        if (heldSoup != null)
        {
            heldSoup.transform.position = holdPoint.position;
            heldSoup.transform.rotation = holdPoint.rotation;
        }
    }

    private void TryPickUpSoup()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxPickupRange, LayerMask.GetMask("Interactable")))
        {
            if (hit.transform.CompareTag("Can"))
            {
                PickupSoup(hit.transform.gameObject);
            }
        }
    }

    private void PickupSoup(GameObject soup)
    {
        if (heldSoup != null) return;

        heldSoup = soup;
        heldRb = soup.GetComponent<Rigidbody>();
        heldCol = soup.GetComponent<Collider>();

        if (heldRb != null) heldRb.isKinematic = true;
        if (heldCol != null) heldCol.enabled = false;

        soup.transform.SetParent(holdPoint);
        soup.transform.localPosition = Vector3.zero;
        soup.transform.localRotation = Quaternion.identity;
        soup.transform.localScale = heldScale;

        soup.transform.up = Vector3.up; // upright
    }

    private void DropSoup()
    {
        if (heldSoup == null) return;

        heldSoup.transform.SetParent(null);
        heldSoup.transform.localScale = droppedScale;

        if (heldRb != null) heldRb.isKinematic = false;
        if (heldCol != null) heldCol.enabled = true;

        heldSoup = null;
        heldRb = null;
        heldCol = null;
    }

    private void ConsumeSoup()
    {
        if (heldSoup == null) return;

        PrimordialSoup.Instance.StartSpeedBoost();

        Destroy(heldSoup);
        heldSoup = null;
        heldRb = null;
        heldCol = null;
    }

    // Utility
    public bool IsHoldingSoup() => heldSoup != null;
}