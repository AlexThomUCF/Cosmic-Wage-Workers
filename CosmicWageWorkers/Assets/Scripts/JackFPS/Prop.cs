using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Prop : MonoBehaviour
{
    public Animator animator;
    public GameObject exitArea;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        exitArea = GameObject.Find("Exit");
    }
    void Start()
    {
        MoveTowardsExit();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MoveTowardsExit()
    {
        StartCoroutine(StartAnimation());
       
    }

    IEnumerator StartAnimation()
    {
        animator.SetTrigger("Play");
        yield return new WaitForSeconds(4f);
   
    }
}
