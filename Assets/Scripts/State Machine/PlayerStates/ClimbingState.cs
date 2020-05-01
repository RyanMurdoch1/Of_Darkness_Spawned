using UnityEngine;

public class ClimbingState : State
{
    private float _verticalMovement;
    private readonly CharacterController _character;
    private readonly float _climbSpeed;
    private static readonly int Climbing = Animator.StringToHash("Climbing");

    public ClimbingState(float climbSpeed, CharacterController character)
    {
        _climbSpeed = climbSpeed;
        _character = character;
    }

    public override void Enter()
    {
        base.Enter();
        _character.animator.SetBool(Climbing, true);
    }
    
    public override void HandleInput()
    {
        _verticalMovement = Input.GetAxisRaw("Vertical") * _climbSpeed;

        if (Input.GetButtonDown("Jump"))
        {
           _character.characterStateMachine.ChangeState(_character.jumpingState);
        }
    }

    public override void PhysicsUpdate() => _character.characterMotor.MoveVertical(_verticalMovement);
    
    public override void Exit() => _character.animator.SetBool(Climbing, false);
}
