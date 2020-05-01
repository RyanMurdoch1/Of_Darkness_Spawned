using System.Collections;
using UnityEngine;

public class EnvironmentDestructable : MonoBehaviour, ITakeDamage
{
    [SerializeField] private int health;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public void TakeDamage(int damage, Vector2 attackLocation)
    {
        health -= damage;
        if (health > 0) return;
        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.125f);
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(0.125f);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.125f);
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(0.125f);
        Perish();
    }

    public void Perish() => gameObject.SetActive(false);
    
    private void OnTriggerEnter2D(Collider2D environmentTrigger)
    {
        if (environmentTrigger.CompareTag("Weapon"))
        {
           TakeDamage(environmentTrigger.GetComponent<IDealDamage>().damageAmount, environmentTrigger.transform.position);
        }
    }
}
