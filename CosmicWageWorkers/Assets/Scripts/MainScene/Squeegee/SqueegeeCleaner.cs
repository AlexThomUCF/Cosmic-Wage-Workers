using UnityEngine;

public class SqueegeeCleaner : MonoBehaviour
{
    public GameObject cameraOBJ;
    public float cleanRange = 3f;

    public SqueegeePickup squeegeePickup;
    public WindowMessManager messManager;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable() => controls.Gameplay.Enable();
    private void OnDisable() => controls.Gameplay.Disable();

    private void Update()
    {
        if (!squeegeePickup.IsHoldingSqueegee()) return;

        if (controls.Gameplay.Use.WasPressedThisFrame())
        {
            TryClean();
        }
    }

    private void TryClean()
    {
        RaycastHit hit;

        if (Physics.Raycast(cameraOBJ.transform.position, cameraOBJ.transform.forward, out hit, cleanRange, LayerMask.GetMask("Interactable")))
        {
            if (hit.transform.CompareTag("Goo"))
            {
                messManager.RemoveGoo(hit.transform.gameObject);
            }
        }
    }
}