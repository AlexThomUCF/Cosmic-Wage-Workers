using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Battery : MonoBehaviour, IInteraction
{
    public GameObject Item;
    public BoxCollider Collider;
    // public AudioSource mcGuffinSound;
    public bool newBattery = false;
    // Start is called before the first frame update

    public void Start()
    {
        Collider = GetComponent<BoxCollider>();
    }

    public UnityEvent onInteract { get; set; } = new UnityEvent();
   
    public void Interact()
    {
        FlashLight flashlight = FindAnyObjectByType<FlashLight>();
        if (flashlight != null)
        {
            flashlight.ReplenishBattery();
        }

        Debug.Log("New battery");
        newBattery = true;
        Collider.enabled = false;
        Item.SetActive(false);
    }
}
