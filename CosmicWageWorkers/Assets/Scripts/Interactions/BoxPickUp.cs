using UnityEngine;
using UnityEngine.Events;

public class BoxPickUp : MonoBehaviour
{
    [Header("Pick Up Settings")]
    public float maxPickupRange = 3f;
    public GameObject cameraOBJ;
    public Transform holdPoint;

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

        if (!isHolding) TryPickUp();
        else DropBox();
    }

    private void TryPickUp()
    {
        RaycastHit hit;
        if (!Physics.Raycast(cameraOBJ.transform.position, cameraOBJ.transform.forward, out hit, maxPickupRange))
            return;

        if (!hit.transform.CompareTag("Box"))
            return;

        heldBox = hit.transform.gameObject;

        Rigidbody rb = heldBox.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        foreach (Collider c in heldBox.GetComponentsInChildren<Collider>())
            c.enabled = false;

        heldBox.transform.SetParent(holdPoint);
        heldBox.transform.localPosition = Vector3.zero;
        heldBox.transform.localRotation = Quaternion.identity;

        isHolding = true;
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

        heldBox.transform.SetParent(null);

        Rigidbody rb = heldBox.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        foreach (Collider c in heldBox.GetComponentsInChildren<Collider>())
            c.enabled = true;

        heldBox.transform.position += cameraOBJ.transform.forward * 0.5f;

        heldBox = null;
        isHolding = false;
    }
}
