using System;
using UnityEngine;

public abstract class EnumStateMachine<T, G> : StateMachine<T> where T : Component where G : Enum
{
    public G LatestType { private set; get; }
    public G PreviousType { private set; get; }

    bool hasFirstSetState;
    public void Initialize(T component, G initialStateType)
    {
        InitializeState(component);
        SetState(initialStateType);
    }

    /// <summary>
    /// Set State as Prev State Type 
    /// </summary>
    public void SetState(G type, bool dontCallLeave = false)
    {
        hasFirstSetState = true;
        PreviousType = LatestType;
        LatestType = type;
        SetState(GetState(type), dontCallLeave);
    }

    /// <summary>
    /// Set State as Prev State Type 
    /// </summary>
    /// <param name="alternativeType">use alternative type if previous equal latestType</param>
    public void SetPrevState(G alternativeType)
    {
        if (!PreviousType.Equals(LatestType))
        {
            SetPrevState();
        }
        else
        {
            SetState(alternativeType);
        }
    }
    public void SetPrevState() => SetState(PreviousType);

    public bool SetStateWhenDifference(G type)
    {
        if (!hasFirstSetState || LatestType.Equals(type))
        {
            return false;
        }
        SetState(type);
        return true;
    }

    protected abstract void InitializeState(T component);
    protected abstract BaseState<T> GetState(G type);
}