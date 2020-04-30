using System.Collections;
using UnityEngine;

public class JumpingState : State
{
    private float _horizontalMovement;
    private readonly float _airSpeed;
    private readonly CharacterController _character;
    private readonly CollisionChecker _collisionChecker;
    private readonly CharacterMotor _characterMotor;
    private bool _isGrounded;
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private readonly WaitForSeconds _waitToClear = new WaitForSeconds(0.25f);
    private bool _clearedGround;

    public JumpingState(StateMachine stateMachine, float airSpeed, CollisionChecker collisionChecker, CharacterMotor characterMotor, CharacterController character) : base(stateMachine)
    {
        _airSpeed = airSpeed;
        _collisionChecker = collisionChecker;
        _character = character;
        _characterMotor = characterMotor;
    }

    public override void Enter()
    {
        base.Enter();
        _clearedGround = false;
        _characterMotor.Jump();
        _character.StartCoroutine(ClearGround());
        _character.animator.SetBool(Jumping, true);
    }

    public override void HandleInput()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal") * _airSpeed;

        if (_character.canClimb && Input.GetButtonDown("Climb"))
        {
            StateMachine.ChangeState(_character.climbingState);
        }
    }

    public override void PhysicsUpdate()
    {
        if (_collisionChecker.CheckForGround() && _clearedGround)
        {
            StateMachine.ChangeState(_character.standingState);
        }
        
        _characterMotor.MoveHorizontal(_horizontalMovement);
    }

    public override void Exit()
    {
        _character.animator.SetBool(Jumping, false); 
    }

    private IEnumerator ClearGround()
    {
        yield return _waitToClear;
        _clearedGround = true;
    }
}
