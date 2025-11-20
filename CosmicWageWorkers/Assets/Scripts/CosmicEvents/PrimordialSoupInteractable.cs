using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PrimordialSoupInteractable : MonoBehaviour
{
    private PlayerControls controls;
    private bool playerNearby = false;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }

    private void Update()
    {
        if (!playerNearby) return;

        if (controls.Gameplay.Interact.IsPressed())
        {
            // Call the manager in the scene
            PrimordialSoup.Instance.StartSpeedBoost();
            Destroy(gameObject); // Remove prefab
        }
    }
}
