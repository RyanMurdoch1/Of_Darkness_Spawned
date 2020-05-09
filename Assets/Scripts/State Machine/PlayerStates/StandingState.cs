using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class StandingState : State
{
    private const float MovementTolerance = 0.01f;
    private readonly float _walkSpeed;
    private readonly PlayerCharacter _character;
    private readonly CollisionChecker _collisionChecker;
    private bool _isGrounded;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private bool _movementStopped;

    public StandingState(float walkSpeed, CollisionChecker collisionChecker, PlayerCharacter character)
    {
        _character = character;
        _walkSpeed = walkSpeed;
        _collisionChecker = collisionChecker;
    }

    public override void Enter()
    {
        base.Enter();
        _character.playerControls.Player.Attack.performed += Attack;
        _character.playerControls.Player.Roll.performed += Roll;
        _character.playerControls.Player.Jump.performed += Jump;
        _character.playerControls.Player.ChangeWeapon.performed += ChangeWeapon;
        _character.playerControls.Player.MoveVertical.performed += StartClimbing;
    }

    public override void Exit()
    {
        _character.playerControls.Player.Attack.performed -= Attack;
        _character.playerControls.Player.Roll.performed -= Roll;
        _character.playerControls.Player.Jump.performed -= Jump;
        _character.playerControls.Player.ChangeWeapon.performed -= ChangeWeapon;
        _character.playerControls.Player.MoveVertical.performed -= StartClimbing;
    }
    
    private void Attack(InputAction.CallbackContext context) => _character.characterStateMachine.ChangeState(_character.attackState);

    private void Roll(InputAction.CallbackContext context) => ChangeState(_character.rollState, true);

    private void Jump(InputAction.CallbackContext context)
    {
        ChangeState(_character.jumpingState, true);
    }

    private void StartClimbing(InputAction.CallbackContext context)
    {
        if (Math.Abs(_character.movementTracker.verticalMoveValue - 1) < MovementTolerance && _character.canClimb)
        {
            ChangeState(_character.climbingState, true);
        }
    }
    
    private void ChangeWeapon(InputAction.CallbackContext context)
    {
        _character.characterMotor.FreezeMovement();
        _character.characterStateMachine.ChangeState(_character.shootingState);
    }
    
    private void ChangeState(State playerState, bool mustBeGrounded)
    {
        if (mustBeGrounded && !_isGrounded) return;
        _character.characterMotor.ResumeMovement();
        _character.characterStateMachine.ChangeState(playerState);
    }

    public override void PhysicsUpdate()
    {
        _isGrounded = _collisionChecker.CheckForGround();
        _character.characterMotor.MoveHorizontal(_character.movementTracker.horizontalMoveValue * _walkSpeed);
        CheckForMovement();
    }

    private void CheckForMovement()
    {
        _character.animator.SetFloat(Speed, Mathf.Abs(_character.movementTracker.horizontalMoveValue));

        if (Math.Abs(_character.movementTracker.horizontalMoveValue) < MovementTolerance && _isGrounded)
        {
            _character.characterMotor.FreezeMovement();
        }
        else
        {
            _character.characterMotor.ResumeMovement();
        }
    }
}
