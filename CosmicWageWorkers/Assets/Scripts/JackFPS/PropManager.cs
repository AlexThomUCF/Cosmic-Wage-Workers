using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PropManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform[] locations;
    public GameObject[] shelfItems;

    [Header("Spawn settings")]
    [SerializeField] private int spawnCount = 10;
    [SerializeField] private float spawnRate = .75f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        randomSpawn();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void randomSpawn()
    {
        StartCoroutine(WaitFunction());

    }

    IEnumerator WaitFunction()
    {
        yield return new WaitForSeconds(spawnRate);

        for (int i = 0; i < spawnCount; i++)
        {
            Transform spawnPoint = locations[Random.Range(0, locations.Length)];

            GameObject prefab = shelfItems[Random.Range(0, shelfItems.Length)];

            yield return new WaitForSeconds(spawnRate);

            GameObject spawnedObj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

            LookAtPlayer lookScript = spawnedObj.GetComponent<LookAtPlayer>();
            if (lookScript != null)
            {
                lookScript.player = player.transform;
            }


        }

        
    }
}
