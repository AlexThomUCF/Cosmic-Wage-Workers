using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatchCheck : MonoBehaviour
{
    public string interactionID;
    public AudioSource catchsound;
    public GameObject climaticCamera;

    [Header("Split Settings")]
    public bool isMini = false;
    public GameObject miniPrefab;
    public int splitCount = 4;
    public float splitRadius = 2f;

    private bool triggered = false;

    private static int minisCaught = 0;
    private static int totalMinisSpawned = 0;

    [Header("NavMesh Settings")]
    public string groundAreaName = "Walkable"; // or "Ground" if you made one

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        StartCoroutine(Switch(other.gameObject));
    }

    private IEnumerator Switch(GameObject playerObj)
    {
        if (catchsound != null)
            catchsound.Play();

        yield return new WaitForSecondsRealtime(1f);

        // First catch = split into minis
        if (!isMini)
        {
            SplitIntoMiniVersions();
            gameObject.SetActive(false);
            yield break;
        }

        // Mini catch
        minisCaught++;
        gameObject.SetActive(false);

        if (minisCaught >= totalMinisSpawned)
        {
            if (MiniGameTimer.Instance != null)
                MiniGameTimer.Instance.StopTimer();

            FindObjectOfType<endscene>().PlayAnim();

            if (!string.IsNullOrEmpty(interactionID))
                CustomerManager.MarkInteractionComplete(interactionID);

            FinalMiniGame.miniGameCount++;
            SaveSystem.SaveGame();

            if (climaticCamera != null)
                climaticCamera.SetActive(true);

            playerObj.SetActive(false);
        }
    }

    private void SplitIntoMiniVersions()
    {
        if (miniPrefab == null)
        {
            Debug.LogWarning("Mini prefab is missing!");
            return;
        }

        TeenAI parentAI = GetComponent<TeenAI>();
        if (parentAI == null)
        {
            Debug.LogWarning("Parent TeenAI missing!");
            return;
        }

        minisCaught = 0;
        totalMinisSpawned = splitCount;

        for (int i = 0; i < splitCount; i++)
        {
            // Get a spread direction (first 4 are clean, rest random)
            Vector3 dir;
            if (i == 0) dir = Vector3.forward;
            else if (i == 1) dir = Vector3.back;
            else if (i == 2) dir = Vector3.left;
            else if (i == 3) dir = Vector3.right;
            else
            {
                dir = Random.insideUnitSphere;
                dir.y = 0f;
                dir.Normalize();

                if (dir.sqrMagnitude < 0.01f)
                    dir = Vector3.forward;
            }

            Vector3 desiredPos = transform.position + dir * splitRadius;
            Vector3 spawnPos = desiredPos;

            int areaMask = 1 << NavMesh.GetAreaFromName(groundAreaName);

            // Snap to NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(desiredPos, out hit, 3f, areaMask))
            {
                spawnPos = hit.position;
            }

            GameObject mini = Instantiate(miniPrefab, spawnPos, transform.rotation);
            mini.transform.localScale = transform.localScale * 0.5f;

            // Setup CatchCheck on mini
            CatchCheck miniCatch = mini.GetComponent<CatchCheck>();
            if (miniCatch != null)
            {
                miniCatch.isMini = true;
                miniCatch.interactionID = interactionID;
                miniCatch.climaticCamera = climaticCamera;
                miniCatch.miniPrefab = miniPrefab;
                miniCatch.splitCount = splitCount;
                miniCatch.splitRadius = splitRadius;
            }

            // Setup TeenAI on mini
            TeenAI miniAI = mini.GetComponent<TeenAI>();
            if (miniAI != null)
            {
                miniAI.player = parentAI.player;
                miniAI.runWhenDistanceLessThan = 999f; // always flee
                miniAI.fleeDistance = parentAI.fleeDistance;
                miniAI.repathInterval = parentAI.repathInterval;
                miniAI.rotateSpeed = parentAI.rotateSpeed;
            }

            // Ensure NavMeshAgent starts properly
            NavMeshAgent agent = mini.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(spawnPos);
                agent.isStopped = false;
            }
        }
    }
}