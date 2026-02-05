using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 camPos = Camera.main.transform.position;

        // keep same Y so it doesn't tilt
        camPos.y = transform.position.y;

        transform.LookAt(camPos);
    }
}