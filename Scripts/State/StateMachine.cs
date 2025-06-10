using System;
using UnityEngine;

public class StateMachine<T> where T : Component
{
    public BaseState<T> CurrentState { private set; get; }

    public void SetState(BaseState<T> newState, bool dontCallLeave = false)
    {
        if (!dontCallLeave)
            CurrentState?.LeaveState();
        CurrentState = newState;
        CurrentState?.EnterState();
    }
}
