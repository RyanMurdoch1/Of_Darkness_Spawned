using UnityEngine;

public class Crate : MonoBehaviour, ITakeDamage
{
    [SerializeField] private int health;
    
    public void TakeDamage(int damage, Vector2 attackLocation)
    {
        health -= damage;
        if (health >= 0) return;
        Perish();
    }

    public void Perish()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D environmentCollider)
    {
        if (environmentCollider.CompareTag("Weapon"))
        {
            Debug.Log("Took damage");
           TakeDamage(environmentCollider.GetComponent<IDealDamage>().damageAmount, environmentCollider.transform.position);
        }
    }
}
