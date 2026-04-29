using UnityEngine;

public class TrashDisposal : MonoBehaviour
{
    private PlayerControls controls;
    private TrashPickup trashPickup;
    private GarbageCanManager garbageManager;
    private TrashSpawn trashSpawn;
    // [SerializeField] private TrashDispose trashDisposeAnim;

    private bool playerNearby;

    private void Awake()
    {
        controls = new PlayerControls();
        garbageManager = Object.FindFirstObjectByType<GarbageCanManager>();
        trashSpawn = Object.FindFirstObjectByType<TrashSpawn>();
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
        if (!playerNearby) return;

        if (controls.Gameplay.Use.WasPressedThisFrame())
        {
            if (trashPickup != null && trashPickup.IsHoldingTrash())
            {
                GameObject trash = trashPickup.GetHeldTrash();

                trashPickup.ForceDropTrash();

                if (garbageManager != null)
                    garbageManager.RemoveTrash(trash);

                Destroy(trash);
                Object.FindFirstObjectByType<CustomerManager>()?.OnTaskCompleted();

                SoundEffectManager.Play("TrashDispose");
                trashSpawn.SpawnTrash();
                //trashDisposeAnim.PlayAnimationFromList();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trashPickup = other.GetComponent<TrashPickup>();
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            trashPickup = null;
        }
    }
}