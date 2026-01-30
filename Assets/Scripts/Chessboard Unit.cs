using UnityEngine;

public enum VisualState { Idle, Hover, Hint, Luminous }

public class ChessboardUnit : MonoBehaviour
{
    [Header("状态配置")]
    public VisualState currentState = VisualState.Idle;

    [Header("颜色设置")]
    public Color idleColor = Color.white;
    public Color hoverColor = new Color(1f, 1f, 0f); // 明黄色
    public Color luminousColor = Color.white;
    [ColorUsage(true, true)] public Color glowColor = Color.yellow; // HDR 发光颜色

    [Header("特殊材质")]
    public Material stripeMaterial; // 预先准备好的条纹材质

    private Material originalMaterial;
    private MeshRenderer meshRenderer;
    private Material instanceMaterial;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        // 获取初始材质的副本，避免修改资源文件
        originalMaterial = meshRenderer.material;
        instanceMaterial = Instantiate(originalMaterial);
        meshRenderer.material = instanceMaterial;
    }

    void Update()
    {
        // 演示用：你可以通过逻辑切换状态
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        switch (currentState)
        {
            case VisualState.Idle:
                SetBaseAppearance(idleColor, false);
                break;

            case VisualState.Hover:
                SetBaseAppearance(hoverColor, false);
                break;

            case VisualState.Hint:
                // 切换到条纹材质
                if (stripeMaterial != null && meshRenderer.sharedMaterial != stripeMaterial)
                {
                    meshRenderer.material = stripeMaterial;
                }
                break;

            case VisualState.Luminous:
                SetBaseAppearance(luminousColor, true);
                break;
        }
    }

    private void SetBaseAppearance(Color targetColor, bool isGlowing)
    {
        // 恢复原始材质形状（如果从Hint切换回来）
        if (meshRenderer.sharedMaterial != instanceMaterial)
        {
            meshRenderer.material = instanceMaterial;
        }

        instanceMaterial.color = targetColor;

        if (isGlowing)
        {
            instanceMaterial.EnableKeyword("_EMISSION");
            instanceMaterial.SetColor("_EmissionColor", glowColor * 2.0f); // 增强亮度
        }
        else
        {
            instanceMaterial.DisableKeyword("_EMISSION");
            instanceMaterial.SetColor("_EmissionColor", Color.black);
        }
    }

    // --- 鼠标交互部分 ---

    private void OnMouseEnter()
    {
        // 只有在普通状态下才触发 Hover
        if (currentState == VisualState.Idle)
        {
            currentState = VisualState.Hover;
        }
    }

    private void OnMouseExit()
    {
        // 离开时如果还是 Hover 状态，则恢复 Idle
        if (currentState == VisualState.Hover)
        {
            currentState = VisualState.Idle;
        }
    }

    // 提供一个公共方法供外部逻辑切换 Hint 或 Luminous
    public void SetState(VisualState newState)
    {
        currentState = newState;
    }
}