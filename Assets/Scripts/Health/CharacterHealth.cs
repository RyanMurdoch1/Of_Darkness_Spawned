using System;
using UnityEngine;

/// <summary>
/// Handles player health
/// </summary>
public class CharacterHealth : MonoBehaviour, ITakeDamage
{
    [SerializeField] private int playerHealth;
    private const int MaxHealth = 3;
    public static event Action<int> HealthChanged;
    public static event Action<Vector2> DamagedFromDirection;

    private void OnEnable() => HealthChanged?.Invoke(playerHealth);

    public void TakeDamage(int damage, Vector2 damageDirection)
    {
        playerHealth -= damage;
        DamagedFromDirection?.Invoke(damageDirection);
        HealthChanged?.Invoke(playerHealth);
        if (playerHealth != 0) return;
        Perish();
    }

    public void RestoreHealth(int value)
    {
        if (playerHealth == MaxHealth) return;
        playerHealth += value;
        HealthChanged?.Invoke(playerHealth);
    }

    public void Perish()
    {
        Debug.Log("Player Died");
    }
}
