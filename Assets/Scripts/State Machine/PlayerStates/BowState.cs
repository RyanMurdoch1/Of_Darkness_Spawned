using System;
using System.Collections;
using UnityEngine;

public class BowState : State
{
    #region Variables
    private readonly GameObject _frontArm;
    private readonly GameObject _backArm;
    private readonly ArrowLauncher _launcher;
    private readonly CharacterController _character;
    private float _bowForce;
    private const float BaseForce = 15f;
    private const float ArmScaleAdjustment = 5f;
    private const float MovementThreshold = 0.1f;
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
    private bool _readyToFire;
    #endregion

    public BowState(CharacterController character, GameObject frontArm, GameObject backArm, ArrowLauncher launcher)
    {
        _character = character;
        _frontArm = frontArm;
        _backArm = backArm;
        _launcher = launcher;
        _playerCamera = Camera.main;
    }

    public override void Enter()
    {
        base.Enter();
        if (_character.characterMotor.FacingRight)
        {
            SetPose(_rightScale, ArmScaleAdjustment);
        }
        else
        {
            SetPose(_leftScale, -ArmScaleAdjustment);
        }
        DisplayAndDrawBow(true);
        _character.characterMotor.FreezeMovement();
        _character.StartCoroutine(ChargeBow());
    }
    
    public override void HandleInput()
    {
        if (Input.GetButtonDown($"Attack") && _readyToFire)
        {
            _character.StopAllCoroutines();
            _character.StartCoroutine(FireArrow());
        }

        if (Input.GetButtonDown("Jump"))
        {
            _character.characterStateMachine.ChangeState(_character.jumpingState);
        }

        if (Math.Abs(Input.GetAxisRaw("Horizontal")) > MovementThreshold)
        {
            _character.characterStateMachine.ChangeState(_character.standingState);
        }

        RotateBow();
    }

    private IEnumerator ChargeBow()
    {
        _readyToFire = false;
        BowForce?.Invoke(0);
        for (var i = 1; i < 4; i++)
        {
            yield return ChargeStep(i);
        }
        _readyToFire = true;
    }

    private IEnumerator ChargeStep(int step)
    {
        CameraShake.shakeCamera(0.004f, 0.25f);
        yield return _bowStageWaitTime;
        AudioController.playAudioFile("Tick");
        BowForce?.Invoke(step);
    }
    
    private IEnumerator FireArrow()
    {
        _readyToFire = false;
        AudioController.playAudioFile("Fire Bow");
        _character.animator.SetBool(FiringBow, true);
        _launcher.Launch(BaseForce);
        yield return _bowStageWaitTime;
        _character.animator.SetBool(FiringBow, false);
        _character.StartCoroutine(ChargeBow());
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
        _backArm.transform.rotation = Quaternion.Euler(0, 0, angle);
        _frontArm.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void DisplayAndDrawBow(bool isDrawn)
    {
        _frontArm.SetActive(isDrawn);
        _backArm.SetActive(isDrawn);
        _character.animator.SetBool(DrawingBow, isDrawn);
        DrawBow?.Invoke(isDrawn);
    }
    
    private void SetPose(Vector2 armScale, float adjustmentValue)
    {
        _frontArm.transform.localScale = armScale;
        _backArm.transform.localScale = armScale;
        AdjustCamera?.Invoke(adjustmentValue, 1);
    }

    private void FlipPlayer(Vector2 armScale)
    {
        _character.characterMotor.Flip();
        SetPose(armScale, Math.Abs(armScale.x + 1) < 0.1f ? -5 : 5);
    }

    public override void Exit()
    {
        DisplayAndDrawBow(false);
        _character.animator.SetBool(FiringBow, false);
        _character.StopAllCoroutines();
        AdjustCamera?.Invoke(0, 3);
        CameraShake.shakeCamera(0, 0);
        _character.characterMotor.ResumeMovement();
    }
}