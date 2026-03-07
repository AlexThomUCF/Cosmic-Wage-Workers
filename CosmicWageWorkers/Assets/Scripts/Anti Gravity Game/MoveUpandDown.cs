using UnityEngine;

public class MoveUpandDown : MonoBehaviour
{
    public float yUp = 3f;
    public float yDown = 3f;
    public float startingThreshold = 3f;    
    public bool movingUp;
    public bool movingDown;
    public float moveSpeed = 5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (movingUp)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            yUp -= Time.deltaTime;
            if (yUp <= 0f)
            {
                movingUp = false;
                movingDown = true;
                yUp = startingThreshold;

            }
        }

        if (movingDown)
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
            yDown -= Time.deltaTime;
            if (yDown <= 0f)
            {
                movingUp = true;
                movingDown = false;
                yDown = startingThreshold;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
