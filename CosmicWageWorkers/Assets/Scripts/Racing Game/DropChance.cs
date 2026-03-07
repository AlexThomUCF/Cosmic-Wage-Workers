using System.Collections;
using UnityEngine;

public class DropChance : MonoBehaviour
{
    [Header("Drop Settings")]
    [Range(0f, 1f)]
    public float dropChance = 0.25f;
    public Transform dropPosition;

    [Header("Timing Settings")]
    public float minDelay = 2f; // Minimum wait time before next drop check
    public float maxDelay = 6f; // Maximum wait time before next drop check

    [Header("Item")]
    public GameObject droppingItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DropLoop());
    }

    [Header("Audio")]
    public AudioSource dropSound;

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(DropLoop());
    }

    IEnumerator DropLoop()
    {
        while (true)
        {
            // Wait a random time between minDelay and maxDelay
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            // Determine if an item should drop
            if (Random.value < dropChance)
            {
                Vector3 offset = new Vector3(0f, -0.3f, -2f);
                GameObject clone = Instantiate(droppingItem, dropPosition.position, transform.rotation);
                dropSound.Play();
                Destroy(clone, 3f);
                Debug.Log($"Dropped item after {waitTime:F1}s!");
            }
            else
            {
                Debug.Log($"No drop after {waitTime:F1}s.");
            }
        }
    }
}
