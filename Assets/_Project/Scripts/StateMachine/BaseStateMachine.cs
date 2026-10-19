using System;
using System.Collections.Generic;

public abstract class BaseStateMachine<EState> where EState : Enum
{
    public BaseState<EState> CurrentState { get; protected set; }

    protected Dictionary<EState, BaseState<EState>> stateByEnum = new();
    protected virtual void Initialize(){}
    
    protected void SetInitialState(EState startState)
    {
        CurrentState = stateByEnum[startState];
        CurrentState.EnterState();
    }
    
    public bool Is(EState key)
    {
        if (CurrentState == null)
            return false; 

        return CurrentState.StateKey.Equals(key);
    }
    
    public virtual void SwitchState(EState key)
    {
        EState previousKey  = CurrentState.StateKey;
        CurrentState.ExitState();
        CurrentState = stateByEnum[key];
        CurrentState.EnterState(previousKey);
    }
}
