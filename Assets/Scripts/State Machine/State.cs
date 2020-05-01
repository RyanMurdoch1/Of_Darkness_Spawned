using UnityEngine;

/// <summary>
/// Base State Class
/// </summary>

public abstract class State
{
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
