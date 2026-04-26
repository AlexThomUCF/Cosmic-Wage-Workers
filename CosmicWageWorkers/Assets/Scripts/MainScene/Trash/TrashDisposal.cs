using UnityEngine;

public class TrashDisposal : MonoBehaviour
{
    private PlayerControls controls;
    private TrashPickup trashPickup;
    private GarbageCanManager garbageManager;
   // [SerializeField] private TrashDispose trashDisposeAnim;

    private bool playerNearby;

    private void Awake()
    {
        controls = new PlayerControls();
        garbageManager = Object.FindFirstObjectByType<GarbageCanManager>();
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
                FindObjectOfType<CustomerManager>()?.OnTaskCompleted();

                SoundEffectManager.Play("TrashDispose");
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