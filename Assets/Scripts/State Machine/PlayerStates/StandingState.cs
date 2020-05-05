using System;
using UnityEngine;

public class StandingState : State
{
    private const float MovementTolerance = 0.01f;
    private float _horizontalMovement;
    private readonly float _walkSpeed;
    private readonly PlayerCharacter _character;
    private readonly CollisionChecker _collisionChecker;
    private bool _isGrounded;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private bool _movementStopped;
    public bool isAttacking;

    public StandingState(float walkSpeed, CollisionChecker collisionChecker, PlayerCharacter character)
    {
        _character = character;
        _walkSpeed = walkSpeed;
        _collisionChecker = collisionChecker;
    }

    public override void HandleInput()
    {
        if (Input.GetButtonDown("Attack") && !isAttacking)
        {
            _character.characterStateMachine.ChangeState(_character.attackState);
        }
        
        _horizontalMovement = Input.GetAxisRaw("Horizontal") * _walkSpeed;
        _character.animator.SetFloat(Speed, Mathf.Abs(_horizontalMovement));
        
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            ChangeState(_character.jumpingState);
        }
        
        if (_character.canClimb && Input.GetButtonDown("Climb"))
        {
            ChangeState(_character.climbingState);
        }

        if (Input.GetButtonDown($"Draw Bow") && _isGrounded)
        {
            ChangeState(_character.shootingState);
        }
    }

    private void ChangeState(State playerState)
    {
        _character.characterMotor.ResumeMovement();
        _character.characterStateMachine.ChangeState(playerState);
    }

    public override void PhysicsUpdate()
    {
        _isGrounded = _collisionChecker.CheckForGround();
        _character.characterMotor.MoveHorizontal(_horizontalMovement);
        if (Math.Abs(_horizontalMovement) < MovementTolerance && _isGrounded)
        {
            _character.characterMotor.FreezeMovement();
        }
        else
        {
            _character.characterMotor.ResumeMovement();
        }
    }
}
