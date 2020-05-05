using System;
using UnityEngine;

/// <summary>
/// Handles player 
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    public static event Action<CollectableType, int> PickedUpItem;
    public static event Action<bool> EnteredAreaClimbing;
    public static event Action<int> RestoreHealth;
    public static event Action<int, Vector2> TakeDamage;
    
    private void OnTriggerEnter2D(Collider2D environmentCol)
    {
        if (environmentCol.CompareTag("Climbable"))
        {
            EnteredAreaClimbing?.Invoke(true);
        }

        if (environmentCol.CompareTag("Collectable"))
        {
            var collectable = environmentCol.gameObject.GetComponent<ICollectable>();
            PickedUpItem?.Invoke(collectable.collectableType, collectable.numberToCollect);
            Collect(environmentCol);
        }
        
        if (environmentCol.CompareTag("Health"))
        {
            RestoreHealth?.Invoke(1);
            Collect(environmentCol);
        }

        if (!environmentCol.CompareTag("Weapon")) return;
        var damage = environmentCol.gameObject.GetComponent<IDealDamage>().damageAmount;
        TakeDamage?.Invoke(damage, environmentCol.transform.position);
    }

    private static void Collect(Component environmentCol)
    {
        AudioController.playAudioFile("Collect");
        environmentCol.gameObject.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Climbable")) return;
        EnteredAreaClimbing?.Invoke(false);
    }
}
