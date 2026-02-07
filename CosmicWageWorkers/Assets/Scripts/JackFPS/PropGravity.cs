using Unity.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PropGravity : MonoBehaviour
{
    public bool isActiveExit = false;

    public int escapedPropCount = 0;
    [SerializeField] private int targetAmount = 10;

    public void PropTracker()
    {
        if (escapedPropCount >= targetAmount)
        {
            SceneManager.LoadScene("JackFPS");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActiveExit) return;   //  key line

        if (other.CompareTag("Prop"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb == null)
                rb = other.gameObject.AddComponent<Rigidbody>();

            NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
            if (agent != null)
                agent.enabled = false;

            Vector3 launchDir = new Vector3(0f, 1f, -1f).normalized;
            rb.AddForce(launchDir * 3f, ForceMode.Impulse);
            rb.useGravity = false;

            escapedPropCount++;
            PropTracker();
        }
    }
}

