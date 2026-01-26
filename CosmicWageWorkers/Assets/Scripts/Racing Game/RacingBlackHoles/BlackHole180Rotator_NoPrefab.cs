using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole180Rotator_Only : MonoBehaviour
{
    public string blackHoleBaseName = "Black Hole VFX";

    [Header("Deterministic Selection")]
    public Transform referencePoint; // set this in inspector (e.g., Start Line). Closest hole becomes A.

    [Header("Rotation")]
    public Vector3 holeARotationEuler = Vector3.zero;
    public float holeBYawOffset = 180f;

    IEnumerator Start()
    {
        // wait until after something else spawns them
        yield return null;

        // grab all current holes
        List<GameObject> holes = new List<GameObject>();
        foreach (var go in FindAllGameObjectsInScene())
            if (go != null && go.name != null && go.name.StartsWith(blackHoleBaseName))
                holes.Add(go);

        if (holes.Count < 2)
        {
            Debug.LogWarning($"[BlackHole180Rotator_Only] Found {holes.Count} holes. Need 2.");
            yield break;
        }

        // If 4 exist, rotate just the most recent 2:
        holes.Sort((a, b) => b.GetInstanceID().CompareTo(a.GetInstanceID()));
        GameObject h1 = holes[0];
        GameObject h2 = holes[1];

        // Decide A vs B deterministically
        GameObject holeA = h1;
        GameObject holeB = h2;

        if (referencePoint != null)
        {
            float d1 = (h1.transform.position - referencePoint.position).sqrMagnitude;
            float d2 = (h2.transform.position - referencePoint.position).sqrMagnitude;

            // Closest to referencePoint is ALWAYS A
            if (d2 < d1)
            {
                holeA = h2;
                holeB = h1;
            }
        }
        else
        {
            // Fallback: stable ordering by position (so it's not random)
            // This makes A = "leftmost" (x), then "lowest z" if x ties.
            if (h2.transform.position.x < h1.transform.position.x ||
               (Mathf.Approximately(h2.transform.position.x, h1.transform.position.x) && h2.transform.position.z < h1.transform.position.z))
            {
                holeA = h2;
                holeB = h1;
            }
        }

        // Apply rotations
        holeA.transform.rotation = Quaternion.Euler(holeARotationEuler);
        holeB.transform.rotation = Quaternion.Euler(holeARotationEuler + new Vector3(0f, holeBYawOffset, 0f));
    }

    private static IEnumerable<GameObject> FindAllGameObjectsInScene()
    {
#if UNITY_2023_1_OR_NEWER
        var transforms = Object.FindObjectsByType<Transform>(FindObjectsSortMode.None);
#else
        var transforms = Object.FindObjectsOfType<Transform>();
#endif
        foreach (var t in transforms)
            if (t != null) yield return t.gameObject;
    }
}

