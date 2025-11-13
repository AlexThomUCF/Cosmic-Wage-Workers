using UnityEngine;

public class McGuffinRandom : MonoBehaviour
{
    public Transform[] locations;   // spawn points
    public GameObject mcGuffin;     // the real object
    public GameObject fakeObject;   // the decoys

    private GameObject[] cubes;     // holds all spawned cubes

    void Start()
    {
        placeObjects();   // spawn fakes
        placeMcGuffin();  // replace one with the real thing
    }

    void placeObjects()
    {
        // create array to store spawned fakes
        cubes = new GameObject[locations.Length];

        for (int i = 0; i < locations.Length; i++)
        {
            // spawn a fake cube and store it in array
            cubes[i] = Instantiate(fakeObject, locations[i].position, transform.rotation);
        }
    }

    void placeMcGuffin()
    {
        // pick a random fake cube to replace
        int randomIndex = Random.Range(0, cubes.Length);

        // get that cube’s position
        Vector3 spawnPos = cubes[randomIndex].transform.position;

        // remove the fake cube
        Destroy(cubes[randomIndex]);

        // spawn the real mcGuffin there
        Instantiate(mcGuffin, spawnPos, transform.rotation);
    }
}
