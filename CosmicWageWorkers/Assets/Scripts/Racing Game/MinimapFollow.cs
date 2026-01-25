using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player;
    public float height = 50f;

    void LateUpdate()
    {
        Vector3 newPos = player.position;
        newPos.y += height;
        transform.position = newPos;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        // Remove rotation by player.y if you want a static north-facing map:
        // transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}

