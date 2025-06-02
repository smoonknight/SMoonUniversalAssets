using UnityEngine;

public class StateMachine<T> where T : Component
{
    public BaseState<T> CurrentState { private set; get; }

    public void SetState(BaseState<T> newState)
    {
        CurrentState?.LeaveState();
        CurrentState = newState;
        CurrentState?.EnterState();
    }
}
