using UnityEngine;

public class TrashFloat : MonoBehaviour
{
    private TrashDispose trashDisposeAnim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        trashDisposeAnim = GetComponent<TrashDispose>();
        trashDisposeAnim.PlayAnimationFromList();
    }

    // Update is called once per frame
    void Update()
    {
       

        if (transform.position.y < -400f) 
        {
            Destroy(gameObject);
        }
    }
}
