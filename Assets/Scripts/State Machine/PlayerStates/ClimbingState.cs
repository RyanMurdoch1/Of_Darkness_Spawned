using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbingState : State
{
    private readonly PlayerCharacter _character;
    private readonly float _climbSpeed;
    private static readonly int Climbing = Animator.StringToHash("Climbing");
    private readonly CollisionChecker _collisionChecker;
    
    public ClimbingState(float climbSpeed, PlayerCharacter character, CollisionChecker collisionChecker)
    {
        _climbSpeed = climbSpeed;
        _character = character;
        _collisionChecker = collisionChecker;
    }

    public override void Enter()
    {
        base.Enter();
        _character.animator.SetBool(Climbing, true);
        _character.playerControls.Player.Jump.performed += Jump;
    }

    private void Jump(InputAction.CallbackContext context) => _character.characterStateMachine.ChangeState(_character.jumpingState);

    public override void PhysicsUpdate()
    {
        _character.characterMotor.MoveVertical(_character.movementTracker.verticalValue * _climbSpeed);
        CheckForGrounded();
    }

    private void CheckForGrounded()
    {
        if (_collisionChecker.CheckForGround() && _character.movementTracker.verticalValue == -1)
        {
            _character.characterStateMachine.ChangeState(_character.standingState);
        } 
    }

    public override void Exit()
    {
        _character.animator.SetBool(Climbing, false);
        _character.playerControls.Player.Jump.performed -= Jump;
    }
}
