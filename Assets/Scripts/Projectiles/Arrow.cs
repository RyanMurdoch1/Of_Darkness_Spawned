using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour, IDealDamage
{
    [SerializeField] private int damageToDeal;
    public int damageAmount => damageToDeal;
    public Rigidbody2D arrowRigidbody;
    private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(0.05f);
    private bool _hitObject;

    private void OnCollisionEnter2D(Collision2D collisionObject)
    {
        if (collisionObject.gameObject.CompareTag("Player") || _hitObject) return;
        _hitObject = true;
        AudioController.playAudioClip(3);
        StartCoroutine(DisableDelay());
    }

    private IEnumerator DisableDelay()
    {
        yield return _waitForSeconds;
        gameObject.SetActive(false);
        _hitObject = false;
    }
}
