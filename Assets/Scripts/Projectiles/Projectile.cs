using System.Collections;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, IDealDamage
{
    public int damageAmount {get; set;}
    public Rigidbody2D arrowRigidbody;
    private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(0.05f);
    protected bool hitObject;

    private void OnEnable() => SetDamageAmount();

    protected abstract void SetDamageAmount();

    protected abstract void OnCollisionEnter2D(Collision2D other);

    protected IEnumerator DisableDelay()
    {
        yield return _waitForSeconds;
        gameObject.SetActive(false);
        hitObject = false;
    }
}
