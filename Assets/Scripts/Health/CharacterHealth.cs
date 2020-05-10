using System;
using UnityEngine;

/// <summary>
/// Handles player health
/// </summary>
public class CharacterHealth : ITakeDamage
{
    private int _playerHealth;
    private const int MaxHealth = 3;
    public static event Action<int> HealthChanged;
    public static event Action<Vector2> DamagedFromDirection;

    public CharacterHealth(int playerHealth)
    {
        _playerHealth = playerHealth;
        HealthChanged?.Invoke(_playerHealth);
        PlayerInteraction.RestoreHealth += RestoreHealth;
        PlayerInteraction.TakeDamage += TakeDamage;
    }

    public void TakeDamage(int damage, Vector2 damageDirection)
    {
        _playerHealth -= damage;
        DamagedFromDirection?.Invoke(damageDirection);
        HealthChanged?.Invoke(_playerHealth);
        if (_playerHealth != 0) return;
        Perish();
    }

    private void RestoreHealth(int value)
    {
        if (_playerHealth == MaxHealth) return;
        _playerHealth += value;
        HealthChanged?.Invoke(_playerHealth);
    }

    public void Perish()
    {
        Debug.Log("Player Died");
    }

    private void Unsubscribe()
    {
        PlayerInteraction.RestoreHealth -= RestoreHealth;
        PlayerInteraction.TakeDamage -= TakeDamage;
    }
}
