﻿using System.Collections;
using UnityEngine;

public class JumpingState : State
{
    private float _horizontalMovement;
    private readonly float _airSpeed;
    private readonly PlayerCharacter _character;
    private readonly CollisionChecker _collisionChecker;
    private bool _isGrounded;
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private bool _clearedGround;

    public JumpingState(float airSpeed, CollisionChecker collisionChecker, PlayerCharacter character) 
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
        yield return WaitHelper.QuarterSecond;
        _clearedGround = true;
    }
    
    public override void Exit()
    {
        _character.animator.SetBool(Jumping, false);
        CameraShake.shakeCamera(0.004f, 0.25f);
    }
}
