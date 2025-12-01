using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoxManager))]
public class ShelfProgressEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Clear Shelf Progress"))
        {
            BoxManager manager = (BoxManager)target;
            if (manager != null && manager.stockZones != null)
            {
                ShelfProgressData.ResetAllZones(manager.stockZones.Count);
                Debug.Log("Shelf progress reset!");
            }
        }
    }
}
