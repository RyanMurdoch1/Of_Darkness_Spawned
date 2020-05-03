using System.Collections;
using UnityEngine;

public class DamageReceiver
{
    private readonly ITakeDamage _damageReceiver;
    private readonly SpriteRenderer _spriteRenderer;
    private readonly WaitForSeconds _flashWaitTime = new WaitForSeconds(0.125f);
    
    public DamageReceiver(ITakeDamage damageReceiver, SpriteRenderer spriteRenderer)
    {
        _damageReceiver = damageReceiver;
        _spriteRenderer = spriteRenderer;
    }
    
    public IEnumerator TakeDamage()
    {
        for (var i = 0; i < 2; i++)
        {
            yield return Flash();
        }
    }
    
    public IEnumerator TakeDamageAndDie()
    {
        for (var i = 0; i < 2; i++)
        {
            yield return Flash();
        }
        
        _damageReceiver.Perish();
    }
    
    private IEnumerator Flash()
    {
        _spriteRenderer.enabled = false;
        yield return _flashWaitTime;
        _spriteRenderer.enabled = true;
        yield return _flashWaitTime; 
    }
}
