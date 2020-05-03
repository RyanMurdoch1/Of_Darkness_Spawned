using UnityEngine;

public class ClimbingState : State
{
    private float _verticalMovement;
    private readonly PlayerCharacter _character;
    private readonly float _climbSpeed;
    private static readonly int Climbing = Animator.StringToHash("Climbing");
    private readonly CollisionChecker _collisionChecker;
    private bool _isGrounded;
    
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
    }
    
    public override void HandleInput()
    {
        _verticalMovement = Input.GetAxisRaw("Vertical") * _climbSpeed;

        if (Input.GetButtonDown("Jump"))
        {
           _character.characterStateMachine.ChangeState(_character.jumpingState);
        }

        if (_isGrounded && Input.GetKey(KeyCode.S))
        {
            _character.characterStateMachine.ChangeState(_character.standingState);
        }
    }

    public override void PhysicsUpdate()
    {
        _character.characterMotor.MoveVertical(_verticalMovement);
        _isGrounded = _collisionChecker.CheckForGround();
    }

    public override void Exit() => _character.animator.SetBool(Climbing, false);
}
