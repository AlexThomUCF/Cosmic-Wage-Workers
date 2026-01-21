using UnityEngine;

[ExecuteAlways]
public class AllColor : MonoBehaviour
{
    [Tooltip("Material to apply to this object and all children")]
    public Material material;

    [Tooltip("Include inactive children when applying")]
    public bool includeInactive = true;

    [Tooltip("Use sharedMaterial (editor-friendly) or material (creates instances at runtime)")]
    public bool useSharedMaterial = true;

    // Apply from Inspector via context menu
    [ContextMenu("Apply Material to Children")]
    public void ApplyMaterial()
    {
        if (material == null) return;

        var renderers = GetComponentsInChildren<Renderer>(includeInactive);
        foreach (var r in renderers)
        {
            if (r == null) continue;
            if (useSharedMaterial)
                r.sharedMaterial = material;
            else
                r.material = material;
        }
    }

    // Called when inspector values change in editor
    private void OnValidate()
    {
        ApplyMaterial();
    }

    // Ensure it's applied at runtime start as well
    private void Start()
    {
        ApplyMaterial();
    }
}
