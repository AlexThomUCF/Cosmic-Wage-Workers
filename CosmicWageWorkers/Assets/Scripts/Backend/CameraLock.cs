using UnityEngine;
using Unity.Cinemachine;

public class CameraLock : MonoBehaviour
{
    private CinemachineInputAxisController inputController;

    void Awake()
    {
        inputController = GetComponent<CinemachineInputAxisController>();
    }

    void Update()
    {
        inputController.enabled = !NPC.isInDialogue;
    }
}
