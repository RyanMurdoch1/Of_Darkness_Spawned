using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionalMovementTracker
{
    private readonly PlayerControls _controls;
    public float verticalMoveValue { get; private set; }
    public float horizontalMoveValue { get; private set; }
    private float _verticalAimValue;
    private float _horizontalAimValue;
    public Vector2 mousePosition = new Vector2(600, 350);
    private readonly Camera _camera;

    public DirectionalMovementTracker(PlayerControls controls)
    {
        _controls = controls;
        _controls.Player.MoveHorizontal.Enable();
        _controls.Player.MoveVertical.Enable();
        _controls.Player.CursorPosition.Enable();
        _controls.Player.AimHorizontal.Enable();
        _controls.Player.AimVertical.Enable();
        _controls.Player.MoveHorizontal.performed += TrackHorizontal;
        _controls.Player.MoveVertical.performed += TrackVertical;
        _controls.Player.CursorPosition.performed += MoveCursorWithMouse;
        _controls.Player.AimHorizontal.performed += TrackHorizontalAim;
        _controls.Player.AimVertical.performed += TrackVerticalAim;
        _camera = Camera.main;
    }

    private void TrackVertical(InputAction.CallbackContext context) => verticalMoveValue = context.ReadValue<float>();

    private void TrackHorizontal(InputAction.CallbackContext context) => horizontalMoveValue = context.ReadValue<float>();
    
    private void TrackHorizontalAim(InputAction.CallbackContext context) => _horizontalAimValue = context.ReadValue<float>();
    
    private void TrackVerticalAim(InputAction.CallbackContext context) => _verticalAimValue = context.ReadValue<float>();

    public void SetRightMousePosition()
    {
        mousePosition = new Vector2(0.75f * _camera.scaledPixelWidth, 0.55f * _camera.scaledPixelHeight);
        Mouse.current.WarpCursorPosition(mousePosition);
    }

    public void SetLeftMousePosition()
    {
        mousePosition = new Vector2(0.25f * _camera.scaledPixelWidth, 0.55f * _camera.scaledPixelHeight);
        Mouse.current.WarpCursorPosition(mousePosition);
    }

    public void MoveCursor()
    {
        if (InputDeviceManager.isUsingGamePad)
        {
            MoveCursorWithGamePad();
        }
    }

    private void MoveCursorWithGamePad()
    {
        var pos = new Vector2( _horizontalAimValue * 3, _verticalAimValue * 3);
        mousePosition += pos;
        Mouse.current.WarpCursorPosition(mousePosition); 
    }

    private void MoveCursorWithMouse(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void StopTracking()
    {
        _controls.Player.CursorPosition.performed -= MoveCursorWithMouse;
        _controls.Player.MoveHorizontal.performed -= TrackHorizontal;
        _controls.Player.MoveVertical.performed -= TrackVertical;
        _controls.Player.AimHorizontal.performed -= TrackHorizontalAim;
        _controls.Player.AimVertical.performed -= TrackVerticalAim;
    }
}
