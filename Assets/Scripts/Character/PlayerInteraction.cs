using System;
using UnityEngine;

/// <summary>
/// Handles player 
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CharacterHealth characterHealth;

    public static event Action<CollectableType, int> PickedUpItem;
    
    // TODO: Send damage event instead
    // TODO: Extract out to separate method
    private void OnTriggerEnter2D(Collider2D environmentCol)
    {
        if (environmentCol.CompareTag("Climbable"))
        {
            characterController.canClimb = true;
        }

        if (environmentCol.CompareTag("Collectable"))
        {
            var collectable = environmentCol.gameObject.GetComponent<ICollectable>();
            PickedUpItem?.Invoke(collectable.collectableType, collectable.numberToCollect);
            AudioController.playAudioFile("Collect");
            environmentCol.gameObject.SetActive(false);
        }

        if (!environmentCol.CompareTag("Weapon")) return;
        var damage = environmentCol.gameObject.GetComponent<IDealDamage>().damageAmount;
        characterHealth.TakeDamage(damage, environmentCol.transform.position);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Climbable"))
        {
            characterController.canClimb = false;
        }
    }
}
