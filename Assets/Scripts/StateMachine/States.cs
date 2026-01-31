
// --- 空闲状态 ---



public class IdleState : BaseVisualState
{
    public IdleState(TileVisualStateManager manager) : base(manager) { }
    public override void Enter()
    {
        manager.meshRenderer.material = manager.instanceMaterial;
        manager.hoverEffect.originalMaterial = manager.instanceMaterial;
        //manager.instanceMaterial.DisableKeyword("_EMISSION");
    }
    public override void Update() { }
    public override void Exit() { }
}

// --- 被选中的状态 ---
public class SelectedState : BaseVisualState
{
    public SelectedState(TileVisualStateManager manager) : base(manager) {}
    public override void Enter()
    {
        manager.meshRenderer.material = manager.selectedMaterial;
        manager.hoverEffect.originalMaterial = manager.selectedMaterial;
    }

    public override void Update()
    {
        //manager.meshRenderer.material = manager.selectedMaterial;
    }

    public override void Exit()
    {
    }
}

// --- 提示状态 (条纹) ---
public class HintState : BaseVisualState
{
    public HintState(TileVisualStateManager manager) : base(manager) { }
    public override void Enter()
    {
        // if (manager.stripeMaterial != null)
        // {
        //     manager.hoverEffect.originalMaterial = manager.stripeMaterial; 
        //     manager.meshRenderer.material = manager.stripeMaterial;
        //     manager.instanceMaterial = manager.stripeMaterial;
        // }
    }
    public override void Update() { }
    public override void Exit() { }
}

// --- 发光状态 ---
public class LuminousState : BaseVisualState
{
    public LuminousState(TileVisualStateManager manager) : base(manager) { }
    public override void Enter()
    {
        //暂时把材质改为发光材质
        //manager.meshRenderer.material = manager.instanceMaterial;
        //manager.instanceMaterial.color = manager.luminousColor;
        manager.meshRenderer.material=manager.luminousMaterial;
    }
    public override void Update() { }
    public override void Exit() { }
}