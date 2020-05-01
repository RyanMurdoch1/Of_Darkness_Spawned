using UnityEngine;

/// <summary>
/// Carries out player ground and ceiling checks
/// </summary>
public class CollisionChecker
{
    private readonly Transform _groundCheck;
    private const float CheckRadius = 0.2f;
    private readonly LayerMask _whatIsGround;
    private readonly CharacterController _characterMovement;
    private readonly Collider2D[] _colliders2D = new Collider2D[10];
    private readonly Collider2D _nullCollider;

    public CollisionChecker(Transform groundCheck, LayerMask whatIsGround, CharacterController characterMovement)
    {
        _groundCheck = groundCheck;
        _whatIsGround = whatIsGround;
        _characterMovement = characterMovement;
        _nullCollider = new Collider2D();
    }
    
    public bool CheckForGround()
    {
        Physics2D.OverlapCircleNonAlloc(_groundCheck.position, CheckRadius, _colliders2D, _whatIsGround);
        for (var i = 0; i < _colliders2D.Length; i++)
        {
            if(_colliders2D[0] == _nullCollider) continue;
            _colliders2D[0] = _nullCollider;
            return true;
        }

        return false;
    }
}
