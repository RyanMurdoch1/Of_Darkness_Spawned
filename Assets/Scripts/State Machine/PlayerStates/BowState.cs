using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BowState : State
{
    #region Variables
    private readonly GameObject _frontArm;
    private readonly GameObject _backArm;
    private readonly ProjectileLauncher _launcher;
    private readonly PlayerCharacter _character;
    private const float Tolerance = 0.1f;
    private float _bowForce;
    private const float BaseForce = 15f;
    private const float ArmScaleAdjustment = 5f;
    private static readonly int DrawingBow = Animator.StringToHash("DrawingBow");
    private static readonly int FiringBow = Animator.StringToHash("FiringBow");
    private readonly Vector2 _leftScale = new Vector2(-1, 1);
    private readonly Vector2 _rightScale = new Vector2(1, 1);
    private Vector2 _clampVector;
    private readonly Camera _playerCamera;
    public static event Action<float, float> AdjustCamera;
    public static event Action<bool> DrawBow;
    public static event Action<int> BowForce;
    private bool _readyToFire;
    #endregion

    public BowState(PlayerCharacter character, GameObject frontArm, GameObject backArm, ProjectileLauncher launcher)
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
            _character.movementTracker.SetRightMousePosition();
            SetPose(_rightScale, ArmScaleAdjustment);
        }
        else
        {
            _character.movementTracker.SetLeftMousePosition();
            SetPose(_leftScale, -ArmScaleAdjustment);
        }
        DisplayAndDrawBow(true);
        _character.characterMotor.FreezeMovement();
        _character.StartCoroutine(ChargeBow());
        _character.playerControls.Player.Attack.performed += FireBow;
        _character.playerControls.Player.Roll.performed += Roll;
        _character.playerControls.Player.Jump.performed += Jump;
        _character.playerControls.Player.ChangeWeapon.performed += ChangeWeapon;
        _character.playerControls.Player.MoveHorizontal.performed += ExitBowState;
    }

    private void Jump(InputAction.CallbackContext context) => _character.characterStateMachine.ChangeState(_character.jumpingState);
    
    private void Roll(InputAction.CallbackContext context)
    {
        if (_character.characterMotor.FacingRight && Math.Abs(_character.movementTracker.horizontalMoveValue - -1) < Tolerance)
        {
            FlipPlayer(_leftScale);
        }
        else if (!_character.characterMotor.FacingRight && Math.Abs(_character.movementTracker.horizontalMoveValue - 1) < Tolerance)
        {
            FlipPlayer(_rightScale);
        }
        _character.characterStateMachine.ChangeState(_character.rollState);
    }

    private void ExitBowState(InputAction.CallbackContext context)
    {
        if (Math.Abs(Mathf.Abs(context.ReadValue<float>()) - 1) < Tolerance)
        {
            _character.characterStateMachine.ChangeState(_character.standingState);
        }
    }

    private void ChangeWeapon(InputAction.CallbackContext context) => _character.characterStateMachine.ChangeState(_character.standingState);

    private void FireBow(InputAction.CallbackContext context)
    {
        if (!_readyToFire) return;
        _character.StopAllCoroutines();
        _character.StartCoroutine(FireArrow());
    }
    
    public override void HandleInput() => RotateBow();

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

    private static IEnumerator ChargeStep(int step)
    {
        CameraShake.shakeCamera(0.004f, 0.25f);
        yield return WaitHelper.QuarterSecond;
        AudioController.playAudioFile("Tick");
        BowForce?.Invoke(step);
    }
    
    private IEnumerator FireArrow()
    {
        _readyToFire = false;
        AudioController.playAudioFile("Fire Bow");
        _character.animator.SetBool(FiringBow, true);
        _launcher.Launch(BaseForce);
        yield return WaitHelper.QuarterSecond;
        _character.animator.SetBool(FiringBow, false);
        _character.StartCoroutine(ChargeBow());
    }
    
    private void RotateBow()
    {
        _character.movementTracker.MoveCursor();
        var mouse = _character.movementTracker.mousePosition;
        var screenPoint = _playerCamera.WorldToScreenPoint(_backArm.transform.position);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        if (offset.x < 0 && Math.Abs(_frontArm.transform.localScale.x - _leftScale.x) > Tolerance)
        {
            FlipPlayer(_leftScale);
        }
        else if (offset.x >= 0 && Math.Abs(_frontArm.transform.localScale.x - _rightScale.x) > Tolerance)
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
        SetPose(armScale, Math.Abs(armScale.x + 1) < Tolerance ? -5 : 5);
    }

    public override void Exit()
    {
        DisplayAndDrawBow(false);
        _character.animator.SetBool(FiringBow, false);
        _character.StopAllCoroutines();
        AdjustCamera?.Invoke(0, 3);
        CameraShake.shakeCamera(0, 0);
        _character.playerControls.Player.Attack.performed -= FireBow;
        _character.playerControls.Player.Roll.performed -= Roll;
        _character.playerControls.Player.Jump.performed -= Jump;
        _character.playerControls.Player.ChangeWeapon.performed -= ChangeWeapon;
        _character.playerControls.Player.MoveHorizontal.performed -= ExitBowState;
        _character.characterMotor.ResumeMovement();
    }
}