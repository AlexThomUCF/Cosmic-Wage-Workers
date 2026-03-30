using UnityEngine;
using UnityEngine.Rendering;

public class MoveLeftandRight : MonoBehaviour
{
    public float xLeftThreshold;
    public float xRightThreshold;
    public float startingThreshold;
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
            xLeftThreshold -= Time.deltaTime;
            if (xLeftThreshold <= 0f)
            {
                moveRight = false;
                moveLeft = true;
                xLeftThreshold = startingThreshold;
            }
        }
        else if (moveLeft)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            xRightThreshold -= Time.deltaTime;
            if (xRightThreshold <= 0f)
            {
                moveRight = true;
                moveLeft = false;
                xRightThreshold = startingThreshold;
            }
        }
    }
}

