using System.Globalization;
using TMPro;
using UnityEngine;

/// <summary>
/// Displays player stats
/// </summary>
public class PlayerStatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    private void OnEnable()
    {
        CharacterHealth.HealthChanged += SetHealth;
    }

    private void OnDisable()
    {
        CharacterHealth.HealthChanged -= SetHealth;
    }

    private void SetHealth(int healthValue)
    {
        healthText.text = healthValue.ToString(CultureInfo.InvariantCulture);
    }
}
