using System.Globalization;
using TMPro;
using UnityEngine;

/// <summary>
/// Displays player stats
/// </summary>
public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI amuletText;

    private void OnEnable()
    {
        CharacterHealth.HealthChanged += SetHealth;
        CharacterInventory.CollectableNumberUpdated += CheckCollectables;
    }

    private void OnDisable()
    {
        CharacterHealth.HealthChanged -= SetHealth;
        CharacterInventory.CollectableNumberUpdated -= CheckCollectables;
    }

    private void CheckCollectables(CollectableType collectableType, int value)
    {
        if (collectableType == CollectableType.Coin)
        {
            SetAmulets(value);
        }
    }

    private void SetHealth(int healthValue) => healthText.text = healthValue.ToString(CultureInfo.InvariantCulture);
    
    private void SetAmulets(int amuletValue) => amuletText.text = amuletValue.ToString(CultureInfo.InvariantCulture);
}
