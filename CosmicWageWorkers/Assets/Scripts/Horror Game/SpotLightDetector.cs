using UnityEngine;

public class SpotLightDetector : MonoBehaviour
{
    public Transform enemy;         // Assign your Player transform here
    private Light spotLight;         // The spotlight component
    HorrorAI aiMonster;
    void Start()
    {
        spotLight = GetComponent<Light>();
        aiMonster = FindFirstObjectByType<HorrorAI>();
    }

    void Update()
    {
        // Distance from light to player
        float distance = Vector3.Distance(transform.position, enemy.position);

        // Check if within light range
        if (distance <= spotLight.range)
        {
            // Calculate angle between forward direction and player direction
            Vector3 dirToPlayer = enemy.position - transform.position;
            float angle = Vector3.Angle(transform.forward, dirToPlayer);

            // Check if within spotlight angle
            if (angle <= spotLight.spotAngle / 2f && spotLight.isActiveAndEnabled)
            {
                Debug.Log("In range!");
                aiMonster.monsterAgent.isStopped = true;
                //Freeze The monster here
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        if (spotLight == null)
            spotLight = GetComponent<Light>();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spotLight.range);
    }

}
