using Enums;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TileHoverEffect : MonoBehaviour
{
    // [Header("悬浮描边配置")]
    // public Color hoverOutlineColor = Color.yellow;
    // [Range(1f, 100f)] public float outlineThickness = 5f;
    
    private MeshRenderer meshRenderer;
    public Material originalMaterial;
    public Material hoverMaterial;
    private bool isHovering = false;
    
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    
    void OnMouseEnter()
    {
        if (!Main.MouseEnabled||Main.OperateMode!=OperateMode.ClickMode)
        {
            return;
        }
        if (!isHovering)
        {
            isHovering = true;
            ApplyHoverEffect();
        }
    }
    
    void OnMouseExit()
    {
        if (isHovering)
        {
            isHovering = false;
            RemoveHoverEffect();
        }
    }
    
    private void ApplyHoverEffect()
    {
        if (hoverMaterial != null)
        {
            meshRenderer.material = hoverMaterial;
        }
    }
    
    private void RemoveHoverEffect()
    {
        if (originalMaterial != null)
        {
            meshRenderer.material = originalMaterial;
        }
    }
}