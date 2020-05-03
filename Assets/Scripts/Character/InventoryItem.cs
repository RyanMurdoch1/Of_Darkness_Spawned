using System;

[Serializable]
public class InventoryItem
{
    public CollectableType collectableType;

    public int exposedQuantity;
    
    public int quantity
    {
        get => exposedQuantity;
        set
        {
            exposedQuantity = value;
            CollectableNumberUpdated?.Invoke(collectableType, exposedQuantity);
        }
    }

    public static event Action<CollectableType, int> CollectableNumberUpdated;
}
