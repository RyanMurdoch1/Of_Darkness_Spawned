using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public Dictionary<CollectableType, int> collectables;
    public static event Action<CollectableType, int> CollectableNumberUpdated; 
    
    private void OnEnable()
    {
        collectables = new Dictionary<CollectableType, int>();
        PlayerInteraction.PickedUpItem += AddCollectableToInventory;
    }

    private void OnDisable() => PlayerInteraction.PickedUpItem += AddCollectableToInventory;

    private void AddCollectableToInventory(CollectableType collectableType, int value)
    {
        if (collectables.ContainsKey(collectableType))
        {
            collectables[collectableType] += value;
        }
        else
        {
            collectables.Add(collectableType, value);
        }
        
        CollectableNumberUpdated?.Invoke(collectableType, collectables[collectableType]);
        Debug.Log($"Collected {value.ToString()} {collectableType}");
    }
}
