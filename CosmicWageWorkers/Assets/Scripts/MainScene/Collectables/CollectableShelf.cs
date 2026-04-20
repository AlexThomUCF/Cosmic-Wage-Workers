using UnityEngine;

public class CollectibleShelf : MonoBehaviour
{
    [Header("Shelf Display Objects (9 total)")]
    public GameObject[] shelfItems;

    [Header("Matching IDs (same order as shelfItems)")]
    public string[] collectibleIDs;

    private void Start()
    {
        UpdateShelf();
    }

    public void UpdateShelf()
    {
        if (CollectibleManager.Instance == null) return;

        for (int i = 0; i < shelfItems.Length; i++)
        {
            bool collected = CollectibleManager.Instance.IsCollected(collectibleIDs[i]);
            shelfItems[i].SetActive(collected);
        }
    }
}