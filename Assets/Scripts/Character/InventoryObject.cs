using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu (fileName = "New Inventory", menuName = "Inventory/New Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventoryItem> inventoryItems;

    [Button]
    public void SetNewInventory()
    {
        foreach (var item in inventoryItems)
        {
            item.quantity = item.exposedQuantity;
        }
    }

    [Button]
    public void ResetInventory()
    {
        inventoryItems = new List<InventoryItem>();
    }
}
