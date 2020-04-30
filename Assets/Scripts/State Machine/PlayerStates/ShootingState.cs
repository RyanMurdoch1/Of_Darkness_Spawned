﻿using System;
using System.Collections;
using UnityEngine;

public class ShootingState : State
{
    private readonly GameObject _frontArm;
    private readonly GameObject _backArm;
    private readonly CharacterController _character;
    private readonly CharacterMotor _characterMotor;
    private static readonly int DrawingBow = Animator.StringToHash("DrawingBow");
    private static readonly int FiringBow = Animator.StringToHash("FiringBow");
    private readonly Vector2 _leftScale = new Vector2(-1, 1);
    private readonly Vector2 _rightScale = new Vector2(1, 1);
    private Vector2 _clampVector;
    private readonly Camera _playerCamera;
    public static event Action<float, float> AdjustCamera;
    public static event Action<bool> DrawBow;
    public static event Action<int> BowForce;
    private readonly WaitForSeconds _bowStageWaitTime = new WaitForSeconds(0.25f);

    public ShootingState(StateMachine stateMachine, CharacterController character, CharacterMotor characterMotor, GameObject frontArm, GameObject backArm) : base(stateMachine)
    {
        _character = character;
        _frontArm = frontArm;
        _backArm = backArm;
        _characterMotor = characterMotor;
        _playerCamera = Camera.main;
    }

    public override void Enter()
    {
        base.Enter();
        if (!_characterMotor.FacingRight)
        {
            SetPose(_leftScale, -5);
        }
        else
        {
            SetPose(_rightScale, 5);
        }
        _characterMotor.FreezeMovement();
        DisplayBow(true);
        DrawBow?.Invoke(true);
        _character.StartCoroutine(ChargeBow());
    }

    private IEnumerator ChargeBow()
    {
        BowForce?.Invoke(0);
        CameraShake.shakeCamera(0.002f, 0.25f);
        yield return _bowStageWaitTime;
        BowForce?.Invoke(1);
        CameraShake.shakeCamera(0.004f, 0.25f);
        yield return _bowStageWaitTime;
        CameraShake.shakeCamera(0.006f, 0.25f);
        BowForce?.Invoke(2);
        yield return _bowStageWaitTime;
        BowForce?.Invoke(3);
    }
    
    private void SetPose(Vector2 armScale, float adjustmentValue)
    {
        _frontArm.transform.localScale = armScale;
        _backArm.transform.localScale = armScale;
        AdjustCamera?.Invoke(adjustmentValue, 1);
    }

    public override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _character.StopAllCoroutines();
            _character.StartCoroutine(FireArrow());
        }

        if (Math.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f)
        {
            StateMachine.ChangeState(_character.standingState);
        }
        
        RotateBow();
    }

    private void RotateBow()
    {
        var mouse = Input.mousePosition;
        var screenPoint = _playerCamera.WorldToScreenPoint(_backArm.transform.position);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        if (offset.x < 0 && Math.Abs(_frontArm.transform.localScale.x - _leftScale.x) > 0.1)
        {
            FlipPlayer(_leftScale);
        }
        else if (offset.x >= 0 && Math.Abs(_frontArm.transform.localScale.x - _rightScale.x) > 0.1)
        {
            FlipPlayer(_rightScale);
        }
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        _backArm.transform.rotation =   Quaternion.Euler(0, 0, angle);
        _frontArm.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void FlipPlayer(Vector2 armScale)
    {
        _frontArm.transform.localScale = armScale;
        _backArm.transform.localScale = armScale;
        _characterMotor.Flip();
        AdjustCamera?.Invoke(Math.Abs(armScale.x - (-1)) < 0.1f ? -5 : 5, 1);
    }
    
    private void DisplayBow(bool drawing)
    {
        _frontArm.SetActive(drawing);
        _backArm.SetActive(drawing);
        _character.animator.SetBool(DrawingBow, drawing);
    }

    private IEnumerator FireArrow()
    {
        _character.animator.SetBool(FiringBow, true);
        yield return _bowStageWaitTime;
        _character.animator.SetBool(FiringBow, false);
        _character.StartCoroutine(ChargeBow());
    }

    public override void Exit()
    {
        DisplayBow(false);
        _characterMotor.ResumeMovement();
        AdjustCamera?.Invoke(0, 3);
        DrawBow?.Invoke(false);
        CameraShake.shakeCamera(0, 0);
    }
}
