using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TileHoverEffect : MonoBehaviour
{
    [Header("悬浮描边配置")]
    public Color hoverOutlineColor = Color.yellow;
    [Range(1f, 100f)] public float outlineThickness = 5f;
    
    private MeshRenderer meshRenderer;
    public Material originalMaterial;
    private Material hoverMaterial;
    private bool isHovering = false;
    
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        
        if (meshRenderer.material.HasProperty("_OutlineColor"))
        {
            // 如果原始材质已经是描边材质，直接使用
            originalMaterial = meshRenderer.material;
            hoverMaterial = originalMaterial;
        }
        else
        {
            // 创建独立的悬浮材质
            hoverMaterial = new Material(Shader.Find("Custom/TileHoverOutline"));
        }
    }
    
    void OnMouseEnter()
    {
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
            hoverMaterial.SetColor("_OutlineColor", hoverOutlineColor);
            hoverMaterial.SetFloat("_OutlineThickness", outlineThickness);
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
    
    // 外部设置描边颜色
    public void SetHoverColor(Color color)
    {
        hoverOutlineColor = color;
        if (isHovering)
        {
            hoverMaterial.SetColor("_OutlineColor", hoverOutlineColor);
        }
    }
}