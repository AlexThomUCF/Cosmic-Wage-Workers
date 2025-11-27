using UnityEngine;

public class AiDrops : MonoBehaviour
{
    public float slowSpeed; // how slow the vehicle will slow down

    public CarControl car;
    public VehicleGen4_Arcade currentCar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        car = other.GetComponent<CarControl>();
        currentCar = other.GetComponent<VehicleGen4_Arcade>();

        if (car)
        {
            car.currentSpeed = slowSpeed;
        }
        else if (currentCar)
        {
            Vector3 dir = currentCar.rb.linearVelocity.normalized;
            currentCar.rb.linearVelocity = dir * slowSpeed;
        }
    }
}
