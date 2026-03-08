using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PickupIndicator : MonoBehaviour
{
    [Header("UI Reference")]
    public Image indicator;                  // UI icon in center of screen
    public float scaleSpeed = 5f;
    public float maxScale = 1f;
    public float minScale = 0f;

    [Header("Detection Settings")]
    public Camera playerCamera;
    public float maxDistance = 3f;
    public List<string> interactableTags;    // Tags the indicator reacts to

    private void Update()
    {
        if (indicator == null || playerCamera == null) return;

        float targetScale = IsLookingAtInteractable() ? maxScale : minScale;
        float newScale = Mathf.Lerp(indicator.transform.localScale.x, targetScale, Time.unscaledDeltaTime * scaleSpeed);
        indicator.transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    private bool IsLookingAtInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxDistance))
        {
            foreach (string tag in interactableTags)
            {
                if (hit.transform.CompareTag(tag))
                    return true;
            }
        }
        return false;
    }
}