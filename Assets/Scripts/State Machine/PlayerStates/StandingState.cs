using UnityEngine;

public class StandingState : State
{
    private float _horizontalMovement;
    private readonly float _walkSpeed;
    private readonly CharacterController _character;
    private readonly CollisionChecker _collisionChecker;
    private readonly CharacterMotor _characterMotor;
    private bool _isGrounded;
    private static readonly int Speed = Animator.StringToHash("Speed");

    public StandingState(StateMachine stateMachine, float walkSpeed, CollisionChecker collisionChecker, CharacterMotor characterMotor, CharacterController character) : base(stateMachine)
    {
        _walkSpeed = walkSpeed;
        _collisionChecker = collisionChecker;
        _character = character;
        _characterMotor = characterMotor;
    }

    public override void HandleInput()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal") * _walkSpeed;
        _character.animator.SetFloat(Speed, Mathf.Abs(_horizontalMovement));
        
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            StateMachine.ChangeState(_character.jumpingState);
        }
        
        if (_character.canClimb && Input.GetButtonDown("Climb"))
        {
            StateMachine.ChangeState(_character.climbingState);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            StateMachine.ChangeState(_character.shootingState);
        }
    }

    public override void PhysicsUpdate()
    {
        _isGrounded = _collisionChecker.CheckForGround();
        _characterMotor.MoveHorizontal(_horizontalMovement);
    }
}
