using UnityEngine;

public class MopRespawn : MonoBehaviour
{
    public Vector3 startingPosition;
    private RespawnObjects respawnScript;
    private Collider mopCollider;
    public float boundary;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mopCollider = GetComponent<Collider>();
        respawnScript = GameObject.Find("RespawnManager").GetComponent<RespawnObjects>();
    }

    // Update is called once per frame 
    void Update()
    {
        if (transform.position.y < boundary)
        {
            respawnScript.objectActive = false;
            Destroy(gameObject);
        }

    }
}
