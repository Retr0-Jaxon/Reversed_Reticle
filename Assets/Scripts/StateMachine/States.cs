
// --- 空闲状态 ---
public class IdleState : BaseVisualState
{
    public IdleState(TileVisualStateManager manager) : base(manager) { }
    public override void Enter()
    {
        manager.meshRenderer.material = manager.instanceMaterial;
        manager.instanceMaterial.color = manager.idleColor;
        manager.instanceMaterial.DisableKeyword("_EMISSION");
    }
    public override void Update() { }
    public override void Exit() { }
}

// --- 悬浮状态 ---
// public class HoverState : BaseVisualState
// {
//     public HoverState(TileVisualStateManager manager) : base(manager) { }
//     public override void Enter()
//     {
//         manager.instanceMaterial.color = manager.hoverColor;
//     }
//     public override void Update() { }
//     public override void Exit() { }
// }

// --- 被选中的状态 ---
public class SelectedState : BaseVisualState
{
    public SelectedState(TileVisualStateManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        manager.instanceMaterial.color = manager.selectedColor;
    }

    public override void Update()
    {
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
        if (manager.stripeMaterial != null)
            manager.meshRenderer.material = manager.stripeMaterial;
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
        manager.meshRenderer.material = manager.instanceMaterial;
        manager.instanceMaterial.color = manager.luminousColor;
        manager.instanceMaterial.EnableKeyword("_EMISSION");
        manager.instanceMaterial.SetColor("_EmissionColor", manager.glowColor * 3.0f);
    }
    public override void Update() { }
    public override void Exit() { }
}