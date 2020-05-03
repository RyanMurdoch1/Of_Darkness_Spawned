using UnityEngine;

public class Lantern : DestructableObject
{
    [SerializeField] private CollectableDropper[] collectableDroppers;
    private const int HealthValue = 1;

    protected override void SetHealth()
    {
        health = HealthValue;
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
