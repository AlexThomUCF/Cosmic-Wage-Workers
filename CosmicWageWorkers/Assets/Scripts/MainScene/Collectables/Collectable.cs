using UnityEngine;

public class Collectible : MonoBehaviour
{
    public string collectibleID;

    private InteractableObject interactable;

    private void Awake()
    {
        interactable = GetComponent<InteractableObject>();

        if (interactable != null)
            interactable.onInteract.AddListener(Collect);
    }

    private void Start()
    {
        if (CollectibleManager.Instance != null &&
            CollectibleManager.Instance.IsCollected(collectibleID))
        {
            gameObject.SetActive(false);
        }
    }

    public void Collect()
    {
        CollectibleManager.Instance.Collect(collectibleID);
        gameObject.SetActive(false);
    }
}
