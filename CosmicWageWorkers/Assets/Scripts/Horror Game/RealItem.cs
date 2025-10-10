using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class RealItem : MonoBehaviour, IInteraction
{
    public bool isMcguffin = false;
    public HorrorAI horror;

    public static bool hasItem = false;


    public UnityEvent onInteract { get; set; } = new UnityEvent();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        horror = FindAnyObjectByType<HorrorAI>();
    }

    public void Interact()
    {
        onInteract?.Invoke();

        if (isMcguffin)
        {
            Debug.Log("This is the Mcguffin");
            horror.currentState = HorrorAI.AIState.EnragedState;
            SoundEffectManager.Play("RightItem");
            hasItem = true;
            //set phase2bool = true
            //pop up item
        }
        else if (!isMcguffin)
        {


            Debug.Log("This is NOT the Mcguffin");
            SoundEffectManager.Play("WrongItem");

            //Jumpscare
            //image of wrong item?
        }
    }
    
    
}
