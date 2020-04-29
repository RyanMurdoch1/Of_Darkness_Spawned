using UnityEngine;

/// <summary>
/// Handles character physics
/// </summary>
public class CharacterMotor
{
    private bool _facingRight;
    private readonly CharacterController _characterMovement;
    private readonly Rigidbody2D _rigidbody2D;
    private readonly float _smoothingValue;
    private readonly float _jumpForce;
    private Vector3 _velocity = Vector3.zero;

    public CharacterMotor(CharacterController character, Rigidbody2D rigidbody2D, float jumpForce, float smoothingValue)
    {
        _characterMovement = character;
        _rigidbody2D = rigidbody2D;
        _smoothingValue = smoothingValue;
        _jumpForce = jumpForce;
    }
    
    public void MoveHorizontal(float moveValue)
    {
        Vector3 targetVelocity = new Vector2(moveValue, _rigidbody2D.velocity.y);

        _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, _smoothingValue);

        if (moveValue > 0 && !_facingRight)
        {
            Flip();
        }
        else if (moveValue < 0 && _facingRight)
        {
            Flip();
        }
    }

    public void TakeDamage(Vector2 damageDirection)
    {
        var dir = damageDirection - new Vector2(_characterMovement.gameObject.transform.localPosition.x, _characterMovement.gameObject.transform.localPosition.y);
        dir = dir.normalized;
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.AddForce(-dir * 6f, ForceMode2D.Impulse);
    }

    public void MoveVertical(float moveValue)
    {
        Vector3 targetVelocity = new Vector2(0, moveValue);

        _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, _smoothingValue);
    }

    public void Jump()
    {
        _rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
    }
    
    private void Flip()
    {
        _facingRight = !_facingRight;
        var theScale = _characterMovement.gameObject.transform.localScale;
        theScale.x *= -1;
        _characterMovement.gameObject.transform.localScale = theScale;
    }
}
