using UnityEngine;

public class Arrow : Projectile
{
    protected override void SetDamageAmount()
    {
        damageAmount = 1;
    }

    protected override void OnCollisionEnter2D(Collision2D collisionObject)
    {
        if (collisionObject.gameObject.CompareTag("Player") || hitObject) return;
        hitObject = true;
        AudioController.playAudioFile("Hit");
        StartCoroutine(DisableDelay());
    }
}
