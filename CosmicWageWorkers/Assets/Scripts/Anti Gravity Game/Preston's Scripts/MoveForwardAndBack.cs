using UnityEngine;

public class MoveForwardAndBack : MonoBehaviour
{
    public float zForward = 3f;
    public float zBack = 3f;
    public float startingThreshold = 3f;
    public bool movingForward;
    public bool movingBack;
    public float moveSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (movingForward)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            zForward -= Time.deltaTime;
            if (zForward <= 0f)
            {
                movingForward = false;
                movingBack = true;
                zForward = startingThreshold;
            }
        }
        if (movingBack)
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
            zBack -= Time.deltaTime;
            if (zBack <= 0f)
            {
                movingForward = true;
                movingBack = false;
                zBack = startingThreshold;
            }
        }
    }
}
