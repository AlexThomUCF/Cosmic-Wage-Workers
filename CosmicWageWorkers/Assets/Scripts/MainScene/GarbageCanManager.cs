using UnityEngine;
using System;

public class GarbageCanManager : MonoBehaviour
{
    [Header("Trash Settings")]
    public GameObject trashBagPrefab;
    public Transform[] garbageCanSpawns;

    [HideInInspector]
    public GameObject[] activeTrash;

    public int maxTrash => garbageCanSpawns.Length;

    public event Action OnTrashCountChanged;

    private void Awake()
    {
        activeTrash = new GameObject[garbageCanSpawns.Length];
        SpawnAllTrash();
    }

    public void SpawnAllTrash()
    {
        for (int i = 0; i < garbageCanSpawns.Length; i++)
        {
            if (activeTrash[i] == null && trashBagPrefab != null)
            {
                GameObject bag = Instantiate(trashBagPrefab, garbageCanSpawns[i].position, Quaternion.identity);
                bag.transform.up = Vector3.up;
                activeTrash[i] = bag;
            }
        }

        OnTrashCountChanged?.Invoke();
    }

    public GameObject GetTrashAtCan(int index)
    {
        if (index >= 0 && index < activeTrash.Length)
            return activeTrash[index];
        return null;
    }

    public void RemoveTrashAtCan(int index)
    {
        if (index >= 0 && index < activeTrash.Length)
        {
            activeTrash[index] = null;
            OnTrashCountChanged?.Invoke();
        }
    }

    public void RemoveTrash(GameObject trash)
    {
        for (int i = 0; i < activeTrash.Length; i++)
        {
            if (activeTrash[i] == trash)
            {
                RemoveTrashAtCan(i);
                return;
            }
        }
    }

    public int ActiveTrashCount()
    {
        int count = 0;
        foreach (var bag in activeTrash)
        {
            if (bag != null) count++;
        }
        return count;
    }
}