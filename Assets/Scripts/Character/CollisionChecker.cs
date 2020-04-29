using UnityEngine;

/// <summary>
/// Carries out player ground and ceiling checks
/// </summary>
public class CollisionChecker
{
    private readonly Transform _groundCheck, _ceilingCheck;
    private const float CheckRadius = 0.2f;
    private readonly LayerMask _whatIsGround;
    private readonly CharacterController _characterMovement;

    public CollisionChecker(Transform groundCheck, Transform ceilingCheck, LayerMask whatIsGround, CharacterController characterMovement)
    {
        _groundCheck = groundCheck;
        _ceilingCheck = ceilingCheck;
        _whatIsGround = whatIsGround;
        _characterMovement = characterMovement;
    }
    
    public bool CheckForGround()
    {
        var colliders = Physics2D.OverlapCircleAll(_groundCheck.position, CheckRadius, _whatIsGround);
        for (var i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject == _characterMovement.gameObject) continue;
            return true;
        }

        return false;
    }

    public bool CheckForCeiling()
    {
        return Physics2D.OverlapCircle(_ceilingCheck.position, CheckRadius, _whatIsGround);
    }
}
