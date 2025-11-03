using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private float mouseSensitivity = 400f;
    [SerializeField] private float rotationSmoothness = 10f;

    private float xRotation = 0f;
    private Quaternion targetCameraRotation;
    private Quaternion targetBodyRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock and hide cursor
        targetCameraRotation = transform.localRotation;
        targetBodyRotation = playerBody.rotation;
    }

    void Update()
    {
       /* if(NPC.isInDialogue)
        {
            return; //If player dialogue is active freeze camera movement
        }*/
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        targetCameraRotation = Quaternion.Euler(xRotation, 0f, 0f);
        targetBodyRotation *= Quaternion.Euler(0f, mouseX, 0f);

        // Smoothly rotate camera & body
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetCameraRotation, rotationSmoothness * Time.deltaTime);
        playerBody.rotation = Quaternion.Slerp(playerBody.rotation, targetBodyRotation, rotationSmoothness * Time.deltaTime);
    }
}

