using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] public List<GameObject> holes;
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("MainPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BlackHoleTeleport()
    {
        player.transform.position = holes[Random.Range(1,4)].transform.position;
    }
}
