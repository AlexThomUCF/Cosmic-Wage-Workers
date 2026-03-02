using UnityEngine;
using UnityEngine.Rendering;

public class MoveLeftandRight : MonoBehaviour
{
    public float xLeft;
    public float xRight;
    public float moveSpeed = 5f;

    private int direction = 1; // 1 for right, -1 for left
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);
        if(transform.position.x >= xRight)
        {
            direction = -1; // Change direction to left
        }
        else if(transform.position.x <= xLeft)
        {
            direction = 1; // Change direction to right
        }
    }
}
