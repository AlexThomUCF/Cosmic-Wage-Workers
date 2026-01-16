using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUp : MonoBehaviour, IInteraction
{
    public float maxPickupRange = 5;
    GameObject itemCurrentlyHolding;
    public GameObject cameraOBJ;
    bool isHolding = false;

    public UnityEvent onInteract { get; set; } = new UnityEvent();

    public void Interact()
    {
        Debug.Log("You hit the key");
        onInteract?.Invoke();
        if(!isHolding)
        {
            PickUpItem();
            Debug.Log("Is holding");
        }
        else if(isHolding)
        {
            DropItem();
            Debug.Log("Dropped");
        }
    }

    public void PickUpItem()
    {
        RaycastHit hit;
        if(Physics.Raycast(cameraOBJ.transform.position, cameraOBJ.transform.forward, out hit, maxPickupRange))
        {
            if (hit.transform.tag == "Item")
            {
                if (isHolding) DropItem();
                itemCurrentlyHolding = hit.transform.gameObject;

                foreach(var c in hit.transform.GetComponentsInChildren<Collider>())
                    if(c != null)
                    {
                        c.enabled = false;
                    }
                foreach (var r in hit.transform.GetComponentsInChildren<Rigidbody>())
                    if (r != null)
                    {
                        r.isKinematic = true;
                    }
                itemCurrentlyHolding.transform.parent = transform;
                itemCurrentlyHolding.transform.localPosition = Vector3.zero;
                itemCurrentlyHolding.transform.localEulerAngles = Vector3.zero;

                isHolding = true;
            }
        }
    }

    public void DropItem()
    {
        if (itemCurrentlyHolding == null)
        {
            Debug.LogWarning("Tried to drop item but none is held.");
            return;
        }
            itemCurrentlyHolding.transform.parent = null;

        foreach (var c in itemCurrentlyHolding.transform.GetComponentsInChildren<Collider>())
            if (c != null)
            {
                c.enabled = true;
            }
        foreach (var r in itemCurrentlyHolding.transform.GetComponentsInChildren<Rigidbody>())
            if (r != null)
            {
                r.isKinematic = false;
            }

        isHolding = false;
        RaycastHit hitDown;
        Physics.Raycast(transform.position, -Vector3.up, out hitDown);

        itemCurrentlyHolding.transform.position = hitDown.point + new Vector3(transform.forward.x, 0, transform.forward.z);

       


    }
}
