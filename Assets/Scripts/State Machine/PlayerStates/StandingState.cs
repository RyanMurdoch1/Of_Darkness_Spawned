using UnityEngine;

public class StandingState : State
{
    private float _horizontalMovement;
    private readonly float _walkSpeed;
    private readonly PlayerCharacter _character;
    private readonly CollisionChecker _collisionChecker;
    private bool _isGrounded;
    private static readonly int Speed = Animator.StringToHash("Speed");

    public StandingState(float walkSpeed, CollisionChecker collisionChecker, PlayerCharacter character)
    {
        _character = character;
        _walkSpeed = walkSpeed;
        _collisionChecker = collisionChecker;
    }

    public override void HandleInput()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal") * _walkSpeed;
        _character.animator.SetFloat(Speed, Mathf.Abs(_horizontalMovement));
        
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _character.characterStateMachine.ChangeState(_character.jumpingState);
        }
        
        if (_character.canClimb && Input.GetButtonDown("Climb"))
        {
            _character.characterStateMachine.ChangeState(_character.climbingState);
        }

        if (Input.GetButtonDown($"Draw Bow") && _isGrounded)
        {
            _character.characterStateMachine.ChangeState(_character.shootingState);
        }
    }

    public override void PhysicsUpdate()
    {
        _isGrounded = _collisionChecker.CheckForGround();
        _character.characterMotor.MoveHorizontal(_horizontalMovement);
    }
}
