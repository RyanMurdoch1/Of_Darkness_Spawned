using UnityEngine;

public class Lantern : DestructableObject
{
    [SerializeField] private CollectableDropper collectableDropper;
    private const int HealthValue = 1;

    protected override void SetHealth()
    {
        health = HealthValue;
    }

    public override void Perish()
    {
        collectableDropper.Drop();
        gameObject.SetActive(false);
    }
}
