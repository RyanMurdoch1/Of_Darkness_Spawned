using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public InventoryObject inventory;
    
    private void OnEnable()
    {
        Invoke(nameof(SetStartingInventory), 0.25f);
        PlayerInteraction.PickedUpItem += AddCollectableToInventory;
    }

    private void SetStartingInventory()
    {
        for (var i = 0; i < inventory.inventoryItems.Count; i++)
        {
            inventory.inventoryItems[i].quantity = inventory.inventoryItems[i].exposedQuantity;
        }
    }

    private void OnDisable() => PlayerInteraction.PickedUpItem += AddCollectableToInventory;

    private void AddCollectableToInventory(CollectableType collectableType, int value)
    {
        for (var i = 0; i < inventory.inventoryItems.Count; i++)
        {
            if (inventory.inventoryItems[i].collectableType != collectableType) continue;
            inventory.inventoryItems[i].quantity += value;
            return;
        }
        
        var newItem = new InventoryItem()
        {
            collectableType = collectableType,
            quantity = value
        };

        inventory.inventoryItems.Add(newItem);
        Debug.Log($"Collected {value.ToString()} {collectableType}");
    }
}
