using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    private Camera _mainCamera;
    private const float ShakeAmount = 0.004f;
    private const float ShakeTime = 0.25f;
    private const float HighShake = 0.75f;
    private const float ShakeRepeatRate = 0.01f;
    private const float ShakeMultiplier = 2;

    public delegate void ShakeCamera();
    public static ShakeCamera shakeCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        shakeCamera += Shake;
    }

    private void OnDisable() => shakeCamera -= Shake;

    private void Shake()
    {
        InvokeRepeating(nameof(BeginShake), 0, ShakeRepeatRate);
        Invoke(nameof(StopShake), ShakeTime);
    }
    
    private void BeginShake()
    {
        var camPos = _mainCamera.transform.position;
        var shakeAmountX = Random.value * ShakeAmount * ShakeMultiplier - ShakeAmount;
        var shakeAmountY = Random.value * ShakeAmount * ShakeMultiplier - ShakeAmount;
        camPos.x += shakeAmountX;
        camPos.y += shakeAmountY;
        _mainCamera.transform.position = camPos;
        if (!InputDeviceManager.isUsingGamePad) return;
        Gamepad.current.SetMotorSpeeds(ShakeTime, HighShake);
    }

    private void StopShake()
    {
        CancelInvoke(nameof(BeginShake));
        _mainCamera.transform.localPosition = Vector3.zero;
        if (!InputDeviceManager.isUsingGamePad) return;
        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }
}
