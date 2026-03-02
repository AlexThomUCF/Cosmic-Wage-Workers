using UnityEngine;
using UnityEngine.Rendering;

public class MoveLeftandRight : MonoBehaviour
{
    public float xLeftTimer = 3f;
    public float xRightTimer = 3f;
    public bool moveLeft;
    public bool moveRight;

    public float moveSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moveRight)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            xRightTimer -= Time.deltaTime;
            if (xRightTimer <= 0f)
            {
                moveRight = false;
                moveLeft = true;
                xRightTimer = 3f;
            }
        }
        if (moveLeft)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            xLeftTimer -= Time.deltaTime;
            if (xLeftTimer <= 0f)
            {
                moveRight = true;
                moveLeft = false;
                xLeftTimer = 3f;
            }
        }
    }
}
