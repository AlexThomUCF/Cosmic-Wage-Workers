using UnityEngine;

public class MovingForward : MonoBehaviour
{
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);

        if (transform.position.z >= 887)
        {
            Destroy(gameObject);
        }
    }
}
