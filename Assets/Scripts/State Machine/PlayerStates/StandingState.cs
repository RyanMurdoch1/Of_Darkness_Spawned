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
        if (_character.canClimb && Input.GetAxis("Vertical") > 0.5f)
        {
            ChangeState(_character.climbingState);
        }

        if (!_isGrounded) return;
        
        if (Input.GetButtonDown("Attack") && !isAttacking || Input.GetAxis("Primary Attack") > 0.1f && !isAttacking)
        {
            _character.characterStateMachine.ChangeState(_character.attackState);
        }

        if (Input.GetButtonDown("Roll"))
        {
            ChangeState(_character.rollState);
        }
        
        _horizontalMovement = Input.GetAxisRaw("Horizontal") * _walkSpeed;
        _character.animator.SetFloat(Speed, Mathf.Abs(_horizontalMovement));
        
        if (Input.GetButtonDown("Jump"))
        {
            ChangeState(_character.jumpingState);
        }

        if (!Input.GetButtonDown($"Draw Bow")) return;
        _character.characterMotor.FreezeMovement();
        _character.characterStateMachine.ChangeState(_character.shootingState);
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
        CheckForMovement();
    }

    private void CheckForMovement()
    {
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
