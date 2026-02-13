using UnityEngine;

public class AlarmNode : MonoBehaviour
{
    public int alarmID;
    public AlarmSequenceManager manager;

    public Material idleMat;
    public Material activeMat;

    private MeshRenderer rend;
    private bool isActive = false;

    void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        SetIdle();
    }

    public void SetIdle()
    {
        isActive = false;
        rend.material = idleMat;
    }

    public void SetActive()
    {
        isActive = true;
        rend.material = activeMat;
    }

    public void OnShot()
    {
        if (!isActive) return; // early shots ignored
        manager.RegisterHit(alarmID);
    }
}


