using System.Collections;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    [SerializeField] private int spawnCount = 10;
    public Transform[] locations;
    public GameObject[] shelfItems; 
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
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < spawnCount; i++)
        {
            Transform spawnPoint = locations[Random.Range(0, locations.Length)];
            GameObject prefab = shelfItems[Random.Range(0, shelfItems.Length)];

            yield return new WaitForSeconds(3f);
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        }

        
    }
}
