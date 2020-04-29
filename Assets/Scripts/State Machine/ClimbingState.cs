using UnityEngine;

public class ClimbingState : State
{
    private readonly CharacterMotor _characterMotor;
    private float _verticalMovement;
    private readonly CharacterController _characterMovement;
    private readonly float _climbSpeed;
    private static readonly int Climbing = Animator.StringToHash("Climbing");

    public ClimbingState(StateMachine stateMachine, CharacterMotor characterMotor, float climbSpeed, CharacterController characterMovement) : base(stateMachine)
    {
        _characterMotor = characterMotor;
        _climbSpeed = climbSpeed;
        _characterMovement = characterMovement;
    }

    public override void Enter()
    {
        base.Enter();
        _characterMovement.animator.SetBool(Climbing, true);
    }

    public override void Exit()
    {
        _characterMovement.animator.SetBool(Climbing, false);
    }

    public override void HandleInput()
    {
        _verticalMovement = Input.GetAxisRaw("Vertical") * _climbSpeed;

        if (Input.GetButtonDown("Jump"))
        {
           StateMachine.ChangeState(_characterMovement.jumpingState);
        }
    }

    public override void PhysicsUpdate()
    {
        _characterMotor.MoveVertical(_verticalMovement);
    }
}
