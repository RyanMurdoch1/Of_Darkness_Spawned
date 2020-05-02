using System.Collections;
using UnityEngine;

public class DamageReceiver
{
    private readonly ITakeDamage _damageReceiver;
    private readonly SpriteRenderer _spriteRenderer;
    
    public DamageReceiver(ITakeDamage damageReceiver, SpriteRenderer spriteRenderer)
    {
        _damageReceiver = damageReceiver;
        _spriteRenderer = spriteRenderer;
    }
    
    public IEnumerator TakeDamage()
    {
        _spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.125f);
        _spriteRenderer.enabled = true;
        yield return new WaitForSeconds(0.125f);
        _spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.125f);
        _spriteRenderer.enabled = true;
        yield return new WaitForSeconds(0.125f);
    }

    public IEnumerator TakeDamageAndDie()
    {
        _spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.125f);
        _spriteRenderer.enabled = true;
        yield return new WaitForSeconds(0.125f);
        _spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.125f);
        _spriteRenderer.enabled = true;
        yield return new WaitForSeconds(0.125f);
        _damageReceiver.Perish();
    }
}
