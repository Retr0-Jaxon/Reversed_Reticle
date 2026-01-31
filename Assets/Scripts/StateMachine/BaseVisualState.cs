public abstract class BaseVisualState
{
    protected TileVisualStateManager manager; // 引用已更新

    public BaseVisualState(TileVisualStateManager manager)
    {
        this.manager = manager;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}