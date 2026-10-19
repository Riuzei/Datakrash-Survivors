using System;

public abstract class BaseState<EState> where EState : Enum
{
    public EState StateKey { get; protected set; }
    
    public BaseState(EState key)
    {
        StateKey = key;
    }

    public virtual void EnterState(EState previousState = default) { }
    public virtual void ExitState() { }
}
