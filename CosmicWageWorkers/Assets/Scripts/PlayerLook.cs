using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float minViewDistance = 25f;// minimum amount you can look down 
    [SerializeField] Transform playerBody;

    public float mouseSense = 100f;

    float xRotation = 0f;

    
    
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSense * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSense * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, minViewDistance);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);//camera rotates

        playerBody.Rotate(Vector3.up * mouseX);// player rotations with camera

    }
}
