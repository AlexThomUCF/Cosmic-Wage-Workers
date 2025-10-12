using UnityEngine;
using System.Collections;

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
        if (!isPlayerNearby || isStocking) return;

        // Holding interact
        if (controls.Gameplay.Interact.IsPressed())
        {
            holdTime += Time.deltaTime;
            if (holdTime >= stockTime)
            {
                StartCoroutine(StockShelf());
                holdTime = 0f;
            }
        }
        else if (controls.Gameplay.Interact.WasReleasedThisFrame())
        {
            holdTime = 0f;
        }
    }

    private IEnumerator StockShelf()
    {
        isStocking = true;

    for (int i = 0; i < cubeCount; i++)
    {
        Vector3 pos = startPoint.position + transform.forward * (i * spacing); // Z axis
        Instantiate(cubePrefab, pos, Quaternion.identity, transform);
        SoundEffectManager.Play("StockSound");
        yield return new WaitForSeconds(stockTime / cubeCount);
    }

        isStocking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = true;
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