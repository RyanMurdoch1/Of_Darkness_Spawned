using UnityEngine;

public class Collectable : MonoBehaviour, ICollectable
{
    [SerializeField] private CollectableType typeOfCollectable;
    [SerializeField] private int value;
    
    public CollectableType collectableType => typeOfCollectable;

    public int numberToCollect => value;
}
