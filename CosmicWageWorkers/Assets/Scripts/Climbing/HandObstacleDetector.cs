using UnityEngine;
using System.Collections;

public class HandObstacleDetector : MonoBehaviour
{
    public LayerMask obstacleLayer;

    [Header("Flash Settings")]
    public float flashDuration = 0.08f;
    public int flashCount = 3;

    private Renderer rend;
    private Color originalColor;
    private Collider col;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        originalColor = rend.material.color;
    }

    public bool CheckForObstacle()
    {
        Vector3 center = col.bounds.center;
        Vector3 halfExtents = col.bounds.extents * 0.9f;

        Collider[] hits = Physics.OverlapBox(
            center,
            halfExtents,
            transform.rotation,
            obstacleLayer
        );

        return hits.Length > 0;
    }

    public IEnumerator FlashRed()
    {
        for (int i = 0; i < flashCount; i++)
        {
            rend.material.color = Color.red;
            yield return new WaitForSeconds(flashDuration);

            rend.material.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}