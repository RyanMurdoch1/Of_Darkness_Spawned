using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTransform;
    private float _adjustment;
    private float _targetAdjustment;
    private float _speed;
    
    private void FixedUpdate()
    {
        transform.position = new Vector3(followTransform.position.x + _adjustment, followTransform.position.y, transform.position.z);
        _adjustment = Mathf.Lerp(_adjustment, _targetAdjustment, Time.deltaTime * _speed);

    }

    private void ReceiveAdjustmentValue(float adjustment, float speedMultiplier)
    {
        _targetAdjustment = adjustment;
        _speed = speedMultiplier;
    }

    private void OnEnable()
    {
        ShootingState.AdjustCamera += ReceiveAdjustmentValue;
    }
}
