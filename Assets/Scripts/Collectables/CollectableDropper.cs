using Sirenix.OdinInspector;
using UnityEngine;

public class CollectableDropper : MonoBehaviour
{
    [SerializeField] private CollectableType collectableType;
    [MinMaxSlider(0, 5, true)]
    [SerializeField] private Vector2 dropRange;

    public void Drop()
    {
        var dropNumber = Random.Range(dropRange.x + 1, dropRange.y + 1);
        for (var i = 0; i < dropNumber; i++)
        {
            CollectablePool.spawnObject(collectableType, gameObject.transform.position);
        }
    }
}
