using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, IInteraction

{
    public UnityEvent onInteract;

    UnityEvent IInteraction.onInteract 
    {
        get => onInteract;
        set => onInteract = value;
             
    }

    public void Interact() => onInteract?.Invoke();
     
}