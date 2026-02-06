using Unity.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PropGravity : MonoBehaviour
{
    public int escapedPropCount = 0;
    [SerializeField] private int targetAmount = 10;

    public void Update()
    {
        
    }

    public void PropTracker()
    {
        if(escapedPropCount == targetAmount )
        {
            string mainSceneName = "JackFPS";  // might need to change back to MainScene

            SceneManager.LoadScene(mainSceneName);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Prop"))
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            Debug.Log("Has RB");

            if (rb == null)
            {
                rb = other.gameObject.AddComponent<Rigidbody>();
                Debug.Log("Adding rigidbody");
            }

            NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
            }

            Vector3 launchDir = new Vector3(0f, 1f, -1f).normalized;
            rb.AddForce(launchDir * 3f, ForceMode.Impulse);
            rb.useGravity = false;
            Debug.Log("Everything ran");

            escapedPropCount++;
        }
        PropTracker();
    }
}
