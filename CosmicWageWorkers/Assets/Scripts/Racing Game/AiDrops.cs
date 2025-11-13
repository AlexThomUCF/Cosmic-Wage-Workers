using UnityEngine;

public class AiDrops : MonoBehaviour
{
    public float slowSpeed; // how slow the vehicle will slow down

    public CarControl car;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        car = other.GetComponent<CarControl>();

        if(car)
        {
            car.currentSpeed = slowSpeed;
        }
    }
}
