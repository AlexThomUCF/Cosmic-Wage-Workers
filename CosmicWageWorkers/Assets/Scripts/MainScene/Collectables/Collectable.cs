using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Unique ID")]
    public string collectibleID;

    private InteractableObject interactable;

    private void Awake()
    {
        interactable = GetComponent<InteractableObject>();

        if (interactable != null)
        {
            interactable.onInteract.AddListener(Collect);
        }
    }

    private void Start()
    {
        // Remove if already collected in this playthrough
        if (CollectibleManager.Instance != null &&
            CollectibleManager.Instance.IsCollected(collectibleID))
        {
            gameObject.SetActive(false);
        }
    }

    private void Collect()
    {
        if (CollectibleManager.Instance.IsCollected(collectibleID))
            return;

        CollectibleManager.Instance.Collect(collectibleID);

        gameObject.SetActive(false);

        Debug.Log("Collected: " + collectibleID);
    }
}
