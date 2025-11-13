using UnityEngine;

public class BreakingZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        AiCarController car = other.GetComponent<AiCarController>();

        if(car)
        {
            car.isInsideBraking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        AiCarController car = other.GetComponent<AiCarController>();

        if (car)
        {
            car.isInsideBraking = false;
        }
    }
}
