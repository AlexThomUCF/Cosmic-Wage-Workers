using UnityEngine;

public class CollectableShelf : MonoBehaviour
{
    public GameObject[] shelfObjects;
    public string[] collectibleIDs;

    public float updateRate = 0.2f;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer < updateRate) return;
        timer = 0f;

        if (CollectibleManager.Instance == null) return;

        for (int i = 0; i < collectibleIDs.Length; i++)
        {
            bool collected = CollectibleManager.Instance.IsCollected(collectibleIDs[i]);

            if (shelfObjects[i].activeSelf != collected)
            {
                shelfObjects[i].SetActive(collected);
            }
        }
    }

    public void RefreshShelf()
    {
        for (int i = 0; i < collectibleIDs.Length; i++)
        {
            bool collected = CollectibleManager.Instance.IsCollected(collectibleIDs[i]);
            shelfObjects[i].SetActive(collected);
        }
    }
}