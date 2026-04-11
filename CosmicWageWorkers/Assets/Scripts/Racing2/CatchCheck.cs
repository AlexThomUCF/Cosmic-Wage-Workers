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
            Debug.LogWarning("Parent TeenAI missing on original enemy!");
            return;
        }

        minisCaught = 0;
        totalMinisSpawned = splitCount;

        Vector3[] directions =
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right
        };

        for (int i = 0; i < splitCount; i++)
        {
            Vector3 desiredPos = transform.position + directions[i % directions.Length] * splitRadius;
            Vector3 spawnPos = desiredPos;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(desiredPos, out hit, 3f, NavMesh.AllAreas))
            {
                spawnPos = hit.position;
            }

            GameObject mini = Instantiate(miniPrefab, spawnPos, transform.rotation);
            mini.transform.localScale = transform.localScale * 0.5f;

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

            TeenAI miniAI = mini.GetComponent<TeenAI>();
            if (miniAI != null)
            {
                miniAI.player = parentAI.player;
                miniAI.waypoints = new List<Transform>(parentAI.waypoints);
                miniAI.runWhenDistanceLessThan = 999f;
                miniAI.repathInterval = parentAI.repathInterval;
                miniAI.minWaypointDistanceFromPlayer = parentAI.minWaypointDistanceFromPlayer;
            }

            NavMeshAgent agent = mini.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(spawnPos);
                agent.isStopped = false;
            }
        }
    }
}