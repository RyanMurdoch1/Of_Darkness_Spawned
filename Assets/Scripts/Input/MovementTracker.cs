using UnityEngine.InputSystem;

public class MovementTracker
{
    private readonly PlayerControls _controls;
    public float verticalValue { get; set; }
    public float horizontalValue { get; set; }
    
    public MovementTracker(PlayerControls controls)
    {
        _controls = controls;
        _controls.Player.MoveHorizontal.Enable();
        _controls.Player.MoveVertical.Enable();
        _controls.Player.MoveHorizontal.performed += TrackHorizontal;
        _controls.Player.MoveVertical.performed += TrackVertical;
    }

    private void TrackVertical(InputAction.CallbackContext context)
    {
        verticalValue = context.ReadValue<float>();
    }
    
    private void TrackHorizontal(InputAction.CallbackContext context)
    {
        horizontalValue = context.ReadValue<float>();
    }

    public void StopTracking()
    {
        _controls.Player.MoveHorizontal.performed -= TrackHorizontal;
        _controls.Player.MoveVertical.performed -= TrackVertical;
    }
}
