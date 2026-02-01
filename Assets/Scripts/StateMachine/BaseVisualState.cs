using Enums;

public abstract class BaseVisualState
{
    protected TileVisualStateManager manager; // 引用已更新
    protected BaseVisualStateType stateType;

    public BaseVisualStateType StateType
    {
        get => stateType;
        set => stateType = value;
    }

    public BaseVisualState(TileVisualStateManager manager)
    {
        this.manager = manager;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}