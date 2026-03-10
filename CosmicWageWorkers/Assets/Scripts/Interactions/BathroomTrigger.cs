using UnityEngine;
using UnityEngine.Events;

public class BathroomTrigger : MonoBehaviour, IInteraction
{

    public UnityEvent onInteract { get; set; } = new UnityEvent();
    public bool canTrigger = true;
    public string miniGameToLoad;
    [SerializeField] SceneLoader loader;

    public void Awake()
    {
      
    }
    public void Interact()
    {
        if (canTrigger)
        {
            loader.LoadSceneByName(miniGameToLoad);
            canTrigger = false;
        }
        else if (!canTrigger)
        {
            Debug.Log("Already Interacted");
        }

    }
   
    
}
