using UnityEngine;
using System.Collections;

public class AntiGravity : MonoBehaviour
{
    [Header("Player Settings")]
    private Rigidbody playerRb;
    public float gravityForce = 5f;      // Optional impulse applied when gravity turns off

    [Header("Timing Settings")]
    public float minGravityOffTime = 3f;
    public float maxGravityOffTime = 5f;
    public float gravityOnDuration = 1f; // time between gravity toggles

    private bool gravityActive = false;
    private Coroutine gravityRoutine;

    private void Start()
    {
        playerRb = GameObject.Find("MainPlayer").GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Call this to trigger the anti-gravity event.
    /// </summary>
    public void TriggerAntiGravity()
    {
        if (gravityRoutine == null)
        {
            gravityRoutine = StartCoroutine(GravityToggleRoutine());
        }
    }

    /// <summary>
    /// Call this from the button in the back room to stop the anti-gravity event.
    /// </summary>
    public void StopAntiGravity()
    {
        if (gravityRoutine != null)
        {
            StopCoroutine(gravityRoutine);
            gravityRoutine = null;
            Physics.gravity = new Vector3(0, -9.81f, 0); // restore normal gravity
        }
    }

    private IEnumerator GravityToggleRoutine()
    {
        while (true)
        {
            // Turn gravity off
            Physics.gravity = Vector3.zero;
            gravityActive = true;

            // Optional: give player a small upward push
            if (playerRb != null)
                playerRb.AddForce(Vector3.up * gravityForce, ForceMode.Impulse);

            // Wait random time with gravity off
            float offTime = Random.Range(minGravityOffTime, maxGravityOffTime);
            yield return new WaitForSeconds(offTime);

            // Turn gravity back on
            Physics.gravity = new Vector3(0, -9.81f, 0);
            gravityActive = false;

            // Wait before turning off again
            yield return new WaitForSeconds(gravityOnDuration);
        }
    }

    /// <summary>
    /// Optional check for UI or gameplay systems
    /// </summary>
    public bool IsGravityActive()
    {
        return gravityActive;
    }
}