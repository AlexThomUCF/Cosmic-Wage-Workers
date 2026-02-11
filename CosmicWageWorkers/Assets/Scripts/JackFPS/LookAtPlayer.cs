using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform player;

    public void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {
        if (!player) return;

        // Optional: lock Y so it doesn't tilt up/down
        Vector3 targetPos = player.transform.position;
        targetPos.y = transform.position.y;

        transform.LookAt(targetPos);
    }
}
