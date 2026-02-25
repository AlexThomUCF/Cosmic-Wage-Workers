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

    [Header("UI")]
    public CosmicPhenomenonUIManager uiManager;

    private bool gravityActive = false;
    public bool gravityRoutineOn;
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

        if (uiManager != null) uiManager.ShowAntiGravity(true);
    }

    /// <summary>
    /// Call this from the button in the back room to stop the anti-gravity event.
    /// </summary>
    public void StopAntiGravity()
    {
        if (gravityRoutine != null)
        {
            StopCoroutine(gravityRoutine);
            gravityRoutineOn = false;
            gravityRoutine = null;
            Physics.gravity = new Vector3(0, -9.81f, 0); // restore normal gravity
        }

        if (uiManager != null) uiManager.ShowAntiGravity(false);
    }

    private IEnumerator GravityToggleRoutine()
    {
        while (true)
        {
            gravityRoutineOn = true;

            // If dialogue is active, force normal gravity and wait
            if (DialogueController.Instance != null &&
                DialogueController.Instance.dialoguePanel.activeSelf)
            {
                Physics.gravity = new Vector3(0, -9.81f, 0);
                gravityActive = false;

                yield return new WaitUntil(() =>
                    !DialogueController.Instance.dialoguePanel.activeSelf
                );
            }

            // Turn gravity off
            Physics.gravity = Vector3.zero;
            gravityActive = true;

            if (playerRb != null)
                playerRb.AddForce(Vector3.up * gravityForce, ForceMode.Impulse);

            float offTime = Random.Range(minGravityOffTime, maxGravityOffTime);
            float timer = 0f;

            // Off timer that pauses during dialogue
            while (timer < offTime)
            {
                if (DialogueController.Instance != null &&
                    DialogueController.Instance.dialoguePanel.activeSelf)
                {
                    Physics.gravity = new Vector3(0, -9.81f, 0);
                    gravityActive = false;

                    yield return new WaitUntil(() =>
                        !DialogueController.Instance.dialoguePanel.activeSelf
                    );

                    // Resume anti-gravity properly
                    Physics.gravity = Vector3.zero;
                    gravityActive = true;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            // Restore gravity
            Physics.gravity = new Vector3(0, -9.81f, 0);
            gravityActive = false;

            float onTimer = 0f;

            while (onTimer < gravityOnDuration)
            {
                if (DialogueController.Instance != null &&
                    DialogueController.Instance.dialoguePanel.activeSelf)
                {
                    yield return new WaitUntil(() =>
                        !DialogueController.Instance.dialoguePanel.activeSelf
                    );
                }

                onTimer += Time.deltaTime;
                yield return null;
            }
        }
    }


    public bool IsGravityActive()
    {
        return gravityActive;
    }
}