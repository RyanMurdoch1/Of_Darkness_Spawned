using System.Collections;
using UnityEngine;

public class JumpingState : State
{
    #region Variables
    private float _horizontalMovement;
    private readonly float _airSpeed;
    private readonly CharacterController _character;
    private readonly CollisionChecker _collisionChecker;
    private bool _isGrounded;
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private readonly WaitForSeconds _waitToClear = new WaitForSeconds(0.25f);
    private bool _clearedGround;
    #endregion

    public JumpingState(float airSpeed, CollisionChecker collisionChecker, CharacterController character) 
    {
        _airSpeed = airSpeed;
        _collisionChecker = collisionChecker;
        _character = character;
    }

    public override void Enter()
    {
        base.Enter();
        _clearedGround = false;
        _character.characterMotor.Jump();
        _character.StartCoroutine(ClearGround());
        _character.animator.SetBool(Jumping, true);
    }

    public override void HandleInput()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal") * _airSpeed;

        if (_character.canClimb && Input.GetButtonDown("Climb"))
        {
            _character.characterStateMachine.ChangeState(_character.climbingState);
        }
    }

    public override void PhysicsUpdate()
    {
        if (_collisionChecker.CheckForGround() && _clearedGround)
        {
            _character.characterStateMachine.ChangeState(_character.standingState);
        }
        
        _character.characterMotor.MoveHorizontal(_horizontalMovement);
    }
    
    private IEnumerator ClearGround()
    {
        yield return _waitToClear;
        _clearedGround = true;
    }
    
    public override void Exit() => _character.animator.SetBool(Jumping, false);
}
