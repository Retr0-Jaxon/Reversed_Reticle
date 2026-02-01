using Enums;
using Unity.VisualScripting;
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
    
    private Tile tile;

    // 状态实例
    private BaseVisualState currentState;
    public IdleState IdleState { get; private set; }

    public BaseVisualState CurrentState
    {
        get => currentState;
        set => currentState = value;
    }

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
        
        
        
        tile = GetComponent<Tile>();
        if (tile==null)
        {
            tile=gameObject.AddComponent<Tile>();
        }
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
        if (!Main.MouseEnabled)
        {
            return;
        }

        tile.onClick();
    }

    

    public void OnSelectedEffect()
    {
        // Hint 和 Luminous 状态由外部控制不受点击影响
        TransitionToState(SelectedState);
    }

    public void UnSelectedEffect()
    {
        TransitionToState(IdleState);
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