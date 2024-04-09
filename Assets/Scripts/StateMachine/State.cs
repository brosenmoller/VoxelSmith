using Godot;

public abstract class State<T>
{
    protected StateMachine<T> stateOwner;

    protected T ctx { get { return stateOwner.Controller; } }

    public void Setup(StateMachine<T> stateMachine)
    {
        stateOwner = stateMachine;
    }

    public virtual void OnEnter() { }
    public virtual void OnUpdate(double delta) { }
    public virtual void OnPhysicsUpdate(double delta) { }
    public virtual void UnHandledInput(InputEvent @event) { }
    public virtual void OnExit() { }
}
