using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TileVisualStateManager : MonoBehaviour
{
    [Header("外观配置")]
    public Color idleColor = Color.white;
    public Color selectedColor = Color.yellow;
    public Color luminousColor = Color.white;
    [ColorUsage(true, true)] public Color glowColor = Color.yellow;
    public Material stripeMaterial;

    [HideInInspector] public MeshRenderer meshRenderer;
    [HideInInspector] public Material instanceMaterial;

    // 状态实例
    private BaseVisualState currentState;
    public IdleState IdleState { get; private set; }
    public SelectedState SelectedState { get; private set; }
    public HintState HintState { get; private set; }
    public LuminousState LuminousState { get; private set; }

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        // 创建独立材质实例，防止修改预制体资源
        instanceMaterial = Instantiate(meshRenderer.material);
        meshRenderer.material = instanceMaterial;

        // 初始化状态
        IdleState = new IdleState(this);
        SelectedState = new SelectedState(this);
        HintState = new HintState(this);
        LuminousState = new LuminousState(this);

        // 默认进入 Idle
        TransitionToState(IdleState);
    }

    void Update()
    {
        currentState?.Update();
    }
    
    public void TransitionToState(BaseVisualState newState)
    {
        if (currentState == newState) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    // --- 鼠标交互事件 ---
    private void OnMouseDown()
    {
        // Hint 和 Luminous 状态由外部控制，不受点击影响
        if (currentState == IdleState)
        {
            TransitionToState(SelectedState);
        }
        else if (currentState == SelectedState)
        {
            TransitionToState(IdleState);
        }
    } 

    // 外部调用接口，例如：unit.SetHint(true);
    public void SetHint(bool active) => TransitionToState(active ? HintState : IdleState);
    public void SetLuminous(bool active) => TransitionToState(active ? LuminousState : IdleState);
}