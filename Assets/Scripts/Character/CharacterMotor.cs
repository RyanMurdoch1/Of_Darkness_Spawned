﻿using UnityEngine;

/// <summary>
/// Handles character physics
/// </summary>
public class CharacterMotor
{
    public bool FacingRight;
    private readonly PlayerCharacter _characterMovement;
    private readonly Rigidbody2D _rigidbody2D;
    private readonly float _smoothingValue;
    private readonly float _jumpForce;
    private Vector3 _velocity = Vector3.zero;
    private const int DamageMultiplier = 6;
    private const int OppositeDirection = -1;

    public CharacterMotor(PlayerCharacter character, Rigidbody2D rigidbody2D, float jumpForce, float smoothingValue)
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

        if (moveValue > 0 && !FacingRight)
        {
            Flip();
        }
        else if (moveValue < 0 && FacingRight)
        {
            Flip();
        }
    }

    private void StopMovement()
    {
        _rigidbody2D.velocity = Vector2.zero;
    }

    public void FreezeMovement()
    {
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector2.zero;
    }

    public void ResumeMovement() => _rigidbody2D.isKinematic = false;

    public void TakeDamage(Vector2 damageDirection)
    {
        var dir = damageDirection - new Vector2(_characterMovement.gameObject.transform.localPosition.x, _characterMovement.gameObject.transform.localPosition.y);
        dir = dir.normalized;
        StopMovement();
        _rigidbody2D.AddForce(-dir * DamageMultiplier, ForceMode2D.Impulse);
    }

    public void MoveVertical(float moveValue)
    {
        Vector3 targetVelocity = new Vector2(0, moveValue);

        _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, _smoothingValue);
    }

    public void Jump() => _rigidbody2D.AddForce(new Vector2(0f, _jumpForce));

    public void Roll()
    {
        StopMovement();
        var rollForce = _jumpForce / 1.5f;
        rollForce = FacingRight ? rollForce : -rollForce;
        _rigidbody2D.AddForce(new Vector2(rollForce, 0));
    }

    public void Flip()
    {
        FacingRight = !FacingRight;
        var theScale = _characterMovement.gameObject.transform.localScale;
        theScale.x *= OppositeDirection;
        _characterMovement.gameObject.transform.localScale = theScale;
    }
}
