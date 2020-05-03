using UnityEngine;

public class Trap : MonoBehaviour, IDealDamage
{
    [SerializeField]
    private int damageToDeal;
    public int damageAmount
    {
        get => damageToDeal;
        set => damageToDeal = value;
    }
}
