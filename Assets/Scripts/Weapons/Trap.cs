using UnityEngine;

public class Trap : MonoBehaviour, IDealDamage
{
    [SerializeField] private int damageToDeal;
    public int damageAmount => damageToDeal;
}
