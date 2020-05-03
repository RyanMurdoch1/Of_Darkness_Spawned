using UnityEngine;

public class TrapArrow : Projectile
{
    protected override void SetDamageAmount()
    {
        damageAmount = 1;
    }

    protected override void OnCollisionEnter2D(Collision2D collisionObject)
    {
        if (hitObject) return;
        hitObject = true;
        AudioController.playAudioFile("Hit");
        StartCoroutine(DisableDelay());
    }
}
