using UnityEngine;

public class TrashSpawn : MonoBehaviour
{
    public GameObject trashPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTrash()
    {
        Instantiate(trashPrefab, transform.position, Quaternion.identity);
    }
}
