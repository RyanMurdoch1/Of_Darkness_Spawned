using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays player stats
/// </summary>
public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] private Image[] healthIcons;
    [SerializeField] private TextMeshProUGUI amuletText;

    private void Awake()
    {
        CharacterHealth.HealthChanged += SetHealth;
        InventoryItem.CollectableNumberUpdated += CheckCollectables;
    }

    private void OnDisable()
    {
        CharacterHealth.HealthChanged -= SetHealth;
        InventoryItem.CollectableNumberUpdated -= CheckCollectables;
    }

    private void CheckCollectables(CollectableType collectableType, int value)
    {
        if (collectableType == CollectableType.Coin)
        {
            SetAmulets(value);
        }
    }

    private void SetHealth(int healthValue)
    {
        for (var i = 0; i < healthIcons.Length; i++)
        {
            healthIcons[i].enabled = i < healthValue;
        }
    }

    private void SetAmulets(int amuletValue) => amuletText.text = amuletValue.ToString(CultureInfo.InvariantCulture);
}
