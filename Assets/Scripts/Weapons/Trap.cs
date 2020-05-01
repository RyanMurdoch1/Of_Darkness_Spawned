using UnityEngine;

public class Trap : MonoBehaviour, IDealDamage
{
    public Trap(int damageAmount)
    {
        this.damageAmount = damageAmount;
    }

    [field: SerializeField]
    public int damageAmount { get; }
}
