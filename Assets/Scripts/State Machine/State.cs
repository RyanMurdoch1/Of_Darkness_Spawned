using UnityEngine;

/// <summary>
/// Base State Class
/// </summary>

public abstract class State
{
    protected readonly StateMachine StateMachine;

    protected State(StateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }
    
    public virtual void Enter()
    {
        Debug.Log(this);
    }

    public virtual void HandleInput()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}
