using UnityEngine;

public class MopRespawn : MonoBehaviour
{
    public Vector3 startingPosition;
    private Collider mopCollider;
    public float boundary;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mopCollider = GetComponent<Collider>();
    }

    // Update is called once per frame 
    void Update()
    {
        if (transform.position.y < boundary)
        {
            mopCollider.enabled = false;
            transform.position = startingPosition;
        }

    }
}
