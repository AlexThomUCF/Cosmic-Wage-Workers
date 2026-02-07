using UnityEngine;

public class ExitManager : MonoBehaviour
{
    public PropGravity[] exits;

    public Transform GetRandomExit()
    {
        if (exits == null || exits.Length == 0)
            return null;

        int index = Random.Range(0, exits.Length);
        return exits[index].transform;
    }
}
