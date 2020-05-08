using UnityEngine;

/// <summary>
/// Player weapon, used to set damage amount.
/// </summary>
public class PlayerWeapon : MonoBehaviour, IDealDamage
{
    public int weaponDamage;

    public int damageAmount
    {
        get => weaponDamage;
        set {}
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Collectable") || col.CompareTag("Health")) return;
        AudioController.playAudioFile("Hit");
    }
}
