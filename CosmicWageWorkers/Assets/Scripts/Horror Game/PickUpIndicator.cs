using UnityEngine;
using UnityEngine.UI;

public class PickUpIndicator : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 3f;     // Max raycast distance
    public Image indicatorImage;
    public float scaleSpeed = 5f;      // Grow/shrink speed
    public float maxScale = 1f;        // Fully visible
    public float minScale = 0f;        // Hidden

    void Update()
    {
        if (playerCamera == null || indicatorImage == null) return;

        bool lookingAtItem = false;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            if (hit.transform.CompareTag("Item")) // Anything tagged "Item"
                lookingAtItem = true;
        }

        // Smoothly grow/shrink indicator
        float targetScale = lookingAtItem ? maxScale : minScale;
        float newScale = Mathf.Lerp(indicatorImage.transform.localScale.x, targetScale, Time.unscaledDeltaTime * scaleSpeed);
        indicatorImage.transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
