using System.Collections;
using UnityEngine;

public class ShelfStocking : MonoBehaviour
{
    [Header("Stocking Settings")]
    public GameObject cubePrefab;
    public int cubeCount = 5;
    public float spacing = 0.4f;
    public float stockTime = 3f;
    public Transform startPoint;

    private bool isPlayerNearby;
    private bool isStocking;
    private float holdTime;

    private PlayerControls controls;
    private BoxPickUp playerPickup;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void Update()
    {
        if (!isPlayerNearby || isStocking || playerPickup == null) return;

        // Only allow stocking if the player is holding a box
        if (!playerPickup.IsHoldingBox()) return;

        if (controls.Gameplay.Stock.IsPressed())
        {
            holdTime += Time.deltaTime;
            if (holdTime >= stockTime)
            {
                StartCoroutine(StockShelf());
                holdTime = 0f;
            }
        }
        else if (controls.Gameplay.Stock.WasReleasedThisFrame())
        {
            holdTime = 0f;
        }
    }

    private IEnumerator StockShelf()
    {
        isStocking = true;

        for (int i = 0; i < cubeCount; i++)
        {
            Vector3 pos = startPoint.position + transform.forward * (i * spacing); // Along Z axis
            Instantiate(cubePrefab, pos, Quaternion.identity, transform);
            SoundEffectManager.Play("StockSound");
            yield return new WaitForSeconds(stockTime / cubeCount);
        }

        isStocking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPickup = other.GetComponent<BoxPickUp>();
            if (playerPickup != null)
                isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            holdTime = 0f;
        }
    }
}
