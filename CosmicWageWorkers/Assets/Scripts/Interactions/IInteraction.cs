using UnityEngine.Events;
public interface IInteraction 
{
    public UnityEvent onInteract {  get; set; }
    public void Interact();
}
