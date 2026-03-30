using UnityEngine;

public class RespawnObjects : MonoBehaviour
{
    public GameObject gravityObject;
    public bool objectActive = false;
    public float xPosition;
    public float yPosition;
    public float zPosition;
    private float respawnTimer = 5f;
    private float spawnInterval = 5f; // Time in seconds between spawns
    private float spawnDelay = 0f; // Time until the next spawn
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (objectActive == false)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0f)
            {
                gravityObject.SetActive(true);
                respawnTimer = 5f;
                Respawning();
                objectActive = true;
            
            }
        }
    }

    private void Respawning() 
    {
        Instantiate(gravityObject);
        gravityObject.transform.position = new Vector3(xPosition, yPosition, zPosition);
    }
}
