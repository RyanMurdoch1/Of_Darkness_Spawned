using UnityEngine;

public class Crate : DestructableObject
{
    [SerializeField] private CollectableDropper[] collectableDroppers;
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
        for (var i = 0; i < collectableDroppers.Length; i++)
        {
            collectableDroppers[i].Drop();
        }
        gameObject.SetActive(false);
    }
}
