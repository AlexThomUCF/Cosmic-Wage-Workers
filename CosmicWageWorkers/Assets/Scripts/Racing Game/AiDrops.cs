using UnityEngine;

public class AiDrops : MonoBehaviour
{
    public float slowSpeed; // how slow the vehicle will slow down

    public KartControllerArcade car;
    public KartControllerArcade currentCar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        KartControllerArcade kart = other.GetComponent<KartControllerArcade>();

        if (kart != null)
        {
            kart.ApplySlow(slowSpeed, 2f); // 2 seconds slowdown
            Debug.Log("Slowed kart!");
        }
    }
}
