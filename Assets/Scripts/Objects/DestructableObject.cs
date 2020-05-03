using UnityEngine;

public abstract class DestructableObject : MonoBehaviour, ITakeDamage
{
    protected int health;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private DamageReceiver _damageReceiver;

    private void OnEnable()
    {
        SetHealth();
        _damageReceiver = new DamageReceiver(this, spriteRenderer);
    }

    protected abstract void SetHealth();
    
    private void OnTriggerEnter2D(Collider2D environmentTrigger)
    {
        if (environmentTrigger.CompareTag("Weapon"))
        {
            TakeDamage(environmentTrigger.GetComponent<IDealDamage>().damageAmount, environmentTrigger.transform.position);
        }
    }

    protected void ChangeSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }
    
    public virtual void TakeDamage(int damage, Vector2 attackLocation)
    {
        health -= damage;
        OnDamaged();
        if (health > 0) return;
        StartCoroutine(_damageReceiver.TakeDamageAndDie());
    }

    protected virtual void OnDamaged(){}

    public abstract void Perish();
}
