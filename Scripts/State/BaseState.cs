using UnityEngine;

public abstract class BaseState<T> where T : Component
{
    protected T component;
    public BaseState(T component)
    {
        this.component = component;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void LeaveState();
}