using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    private Camera _mainCamera;
    private float _shakeAmount;

    public delegate void ShakeCamera(float strength, float time);
    public static ShakeCamera shakeCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        shakeCamera += Shake;
    }

    private void OnDisable() => shakeCamera -= Shake;

    private void Shake(float amount, float length)
    {
        _shakeAmount = amount;
        InvokeRepeating(nameof(BeginShake), 0, 0.01f);
        Invoke(nameof(StopShake), length);
    }
    
    private void BeginShake()
    {
        if (!(_shakeAmount > 0)) return;
        var camPos = _mainCamera.transform.position;
        var shakeAmountX = Random.value * _shakeAmount * 2 - _shakeAmount;
        var shakeAmountY = Random.value * _shakeAmount * 2 - _shakeAmount;
        camPos.x += shakeAmountX;
        camPos.y += shakeAmountY;

        _mainCamera.transform.position = camPos;
    }

    private void StopShake()
    {
        CancelInvoke(nameof(BeginShake));
        _mainCamera.transform.localPosition = Vector3.zero;
    }
}
