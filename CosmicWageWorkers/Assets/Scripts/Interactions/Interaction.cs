using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class Interaction : MonoBehaviour
{
    [SerializeField] private LayerMask interactLayer;
    private PlayerInput playerInput;
    private Transform pTransform;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        pTransform = transform;
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
       if(!Physics.Raycast(pTransform.position + (Vector3.up * 0.3f) + (pTransform.forward * 0.2f), 
           pTransform.forward, out var hit,1.5f,interactLayer)) return;


        if(!hit.transform.TryGetComponent(out InteractableObject interactable)) return; // Check if you can get component from raycast hit
        interactable.Interact();
        Debug.Log("Interact");
    }
}

