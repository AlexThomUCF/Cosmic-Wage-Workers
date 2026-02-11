using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float duration = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    public IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = startPosition + Random.insideUnitSphere;
            yield return null;

        }
        transform.position = startPosition;
    }
}
