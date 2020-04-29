using UnityEngine;

/// <summary>
/// This entity takes damage and perishes
/// </summary>
public interface ITakeDamage
{
    void TakeDamage(int damage, Vector2 attackLocation);
    void Perish();
}
