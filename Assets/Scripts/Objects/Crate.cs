using UnityEngine;

public class Crate : DestructableObject
{
    [SerializeField] private CollectableDropper dropper;
    [SerializeField] private Sprite damagedSprite;
    private const int HealthValue = 2;
    
    protected override void SetHealth()
    {
        health = HealthValue;
    }

    protected override void OnDamaged()
    {
        if (health == 1)
        {
            ChangeSprite(damagedSprite);
        }
    }

    public override void Perish()
    {
        dropper.Drop();
        gameObject.SetActive(false);
    }
}
