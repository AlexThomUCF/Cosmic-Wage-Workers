using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private float interactDistance = 3.0f;
    [SerializeField] private float interactRadius = 0.3f;

    [Header("References")]
    [SerializeField] private Camera playerCamera; // Assign your main camera here

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (!playerCamera)
        {
            playerCamera = Camera.main;
        }
    }

    private void OnEnable()
    {
        playerInput.actions["Interact"].performed += DoInteract;
    }

    private void OnDisable()
    {
        playerInput.actions["Interact"].performed -= DoInteract;
    }

    private void DoInteract(InputAction.CallbackContext context)
    {
        if (!playerCamera) return;

        Vector3 origin = playerCamera.transform.position;
        Vector3 direction = playerCamera.transform.forward;

        RaycastHit hit;
        if (!Physics.SphereCast(origin, interactRadius, direction, out hit, interactDistance, interactLayer))
            return;

        // Search for InteractableObject on hit object, parents, and children
        InteractableObject interactable = hit.transform.GetComponent<InteractableObject>()
            ?? hit.transform.GetComponentInParent<InteractableObject>()
            ?? hit.transform.GetComponentInChildren<InteractableObject>();

        if (interactable == null)
        {
            Debug.Log("No InteractableObject found in object, parents, or children");
            return;
        }

        interactable.Interact();
        Debug.Log("Interacted with: " + interactable.name);
    }

    // Draw gizmos in Scene view to visualize detection range
    private void OnDrawGizmosSelected()
    {
        if (!playerCamera) return;

        Vector3 origin = playerCamera.transform.position;
        Vector3 direction = playerCamera.transform.forward;

        // Draw a line showing the forward direction
        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + direction * interactDistance);

        // Draw spheres along the path to represent the SphereCast
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        int steps = 10;
        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps;
            Vector3 pos = origin + direction * interactDistance * t;
            Gizmos.DrawWireSphere(pos, interactRadius);
        }
    }
}
