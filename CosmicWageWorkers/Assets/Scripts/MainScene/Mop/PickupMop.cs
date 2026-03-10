using UnityEngine;
using UnityEngine.UI;

public class PickupMop : MonoBehaviour
{
    public float pickupRange = 2f;
    public LayerMask mopLayer;
    public GameObject cameraOBJ;
    public AudioSource PickupmopSound;


    private GameObject heldMop;
    private bool isHoldingMop;
    private float mopHeightOffset;

    [HideInInspector]
    public Vector3 cleaningOffset = Vector3.zero;

    public bool IsHoldingMop() => isHoldingMop;

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
        if (Physics.Raycast(cameraOBJ.transform.position, cameraOBJ.transform.forward, out hit, pickupRange, mopLayer))
        {
            if (hit.transform.CompareTag("Mop"))
            {
                heldMop = hit.transform.gameObject;

                Rigidbody rb = heldMop.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.isKinematic = true;

                foreach (Collider c in heldMop.GetComponentsInChildren<Collider>())
                    c.enabled = false;

                heldMop.transform.SetParent(null);
                mopHeightOffset = heldMop.transform.position.y - transform.position.y;

                cleaningOffset = Vector3.zero;

                isHoldingMop = true;
                    PickupmopSound.Play();
            }
        }
    }

    private void LateUpdate()
    {
        if (isHoldingMop && heldMop != null)
        {
            Vector3 forward = cameraOBJ.transform.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 left = -cameraOBJ.transform.right;

            float forwardDistance = 2.2f;
            float leftOffset = 1f;

            Vector3 targetPos = transform.position + forward * forwardDistance + left * leftOffset;
            targetPos.y = transform.position.y + mopHeightOffset;

            heldMop.transform.position = Vector3.Lerp(
                heldMop.transform.position,
                targetPos + heldMop.transform.TransformDirection(cleaningOffset),
                Time.deltaTime * 10f
            );

            heldMop.transform.rotation = Quaternion.Euler(-90f, cameraOBJ.transform.eulerAngles.y, 90f);
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

        Rigidbody rb = heldMop.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;

        foreach (Collider c in heldMop.GetComponentsInChildren<Collider>())
            c.enabled = true;

        heldMop.transform.position += cameraOBJ.transform.forward * 0.5f;

        heldMop = null;
        isHoldingMop = false;
    }
}