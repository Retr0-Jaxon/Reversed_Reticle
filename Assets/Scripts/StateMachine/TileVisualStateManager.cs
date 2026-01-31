using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TileVisualStateManager : MonoBehaviour
{
    [Header("外观配置")]
    public Material instanceMaterial;
    public Material luminousMaterial;
    public Material stripeMaterial;
    public Material selectedMaterial;
    
    [HideInInspector] public TileHoverEffect hoverEffect;
    [HideInInspector] public MeshRenderer meshRenderer;
    //public Material baseMaterial;

    // 状态实例
    private BaseVisualState currentState;
    public IdleState IdleState { get; private set; }
    public SelectedState SelectedState { get; private set; }
    public HintState HintState { get; private set; }
    public LuminousState LuminousState { get; private set; }
    

    void Awake()
    {
        // Color lumiColor;
        // if (ColorUtility.TryParseHtmlString("#CA4AFD", out lumiColor))
        // {
        //     gameObject.GetComponent<TileVisualStateManager>().luminousColor = lumiColor;
        // }
        meshRenderer = GetComponent<MeshRenderer>();
        // 创建独立材质实例防止修改预制体资源
        //instanceMaterial = Instantiate(meshRenderer.material);
        meshRenderer.material = instanceMaterial;
        hoverEffect = GetComponent<TileHoverEffect>();

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
        // Hint 和 Luminous 状态由外部控制不受点击影响
        if (currentState == IdleState)
        {
            TransitionToState(SelectedState);
            //GetComponent<TileHoverEffect>().originalMaterial = instanceMaterial;
            
        }
        else if (currentState == SelectedState)
        {
            TransitionToState(IdleState);
            //GetComponent<TileHoverEffect>().originalMaterial = instanceMaterial;
        }

        var component = GetComponent<Tile>();
        if (component==null)
        {
            component=gameObject.AddComponent<Tile>();
        }
        component.onClick();
    } 

    // 外部调用接口，例如：unit.SetHint(true);


    public void SetHint()
    {
        instanceMaterial=stripeMaterial;
        hoverEffect.originalMaterial=stripeMaterial;
        meshRenderer.material=stripeMaterial;
    }
    public void SetLuminous(bool active) => TransitionToState(active ? LuminousState : IdleState);
}