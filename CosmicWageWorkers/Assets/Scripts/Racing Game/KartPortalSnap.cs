using System.Collections;
using UnityEngine;

public class KartPortalSnap : MonoBehaviour
{
    [Header("Transition")]
    public Transform exitPoint;
    public float snapDuration = 0.15f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Velocity")]
    public float exitSpeedMultiplier = 0.9f;

    private void OnTriggerEnter(Collider other)
    {
        KartControllerArcade kart = other.GetComponent<KartControllerArcade>();
        if (kart == null) return;

        Rigidbody rb = kart.GetComponent<Rigidbody>();
        if (rb == null) return;

        StartCoroutine(SnapKart(kart, rb));
    }

    IEnumerator SnapKart(KartControllerArcade kart, Rigidbody rb)
    {
        // Cache renderers
        MeshRenderer[] renderers = kart.GetComponentsInChildren<MeshRenderer>(true);

        // Hide kart
        foreach (var r in renderers)
            r.enabled = false;

        Vector3 startPos = rb.position;
        Quaternion startRot = rb.rotation;
        Vector3 startVelocity = rb.linearVelocity;

        rb.isKinematic = true;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / snapDuration;
            float eased = moveCurve.Evaluate(t);

            rb.position = Vector3.Lerp(startPos, exitPoint.position, eased);
            rb.rotation = Quaternion.Slerp(startRot, exitPoint.rotation, eased);

            yield return null;
        }

        rb.isKinematic = false;

        float speed = startVelocity.magnitude * exitSpeedMultiplier;
        rb.linearVelocity = exitPoint.forward * speed;
        rb.angularVelocity = Vector3.zero;

        // Show kart again
        foreach (var r in renderers)
            r.enabled = true;
    }
}
