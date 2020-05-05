using System.Collections;
using UnityEngine;

public class AttackState : State
{
    private readonly PlayerCharacter _character;
    private readonly CharacterMotor _motor;
    private readonly GameObject _weaponZone;
    private static readonly int Attacking = Animator.StringToHash("Attacking");

    public AttackState(PlayerCharacter character, CharacterMotor motor, GameObject weaponZone)
    {
        _character = character;
        _motor = motor;
        _weaponZone = weaponZone;
    }

    public override void Enter()
    {
        base.Enter();
        _motor.FreezeMovement();
        _character.standingState.isAttacking = true;
        _character.StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        _character.animator.SetBool(Attacking, true);
        AudioController.playAudioFile("Fire Bow");
        yield return WaitHelper.ThirdSecond;
        _weaponZone.SetActive(true);
        CameraShake.shakeCamera(0.004f, 0.25f);
        yield return WaitHelper.TenthSecond;
        _weaponZone.SetActive(false);
        yield return WaitHelper.TenthSecond;
        _character.animator.SetBool(Attacking, false);
        _character.characterStateMachine.ChangeState(_character.standingState);
        _character.standingState.isAttacking = false;
    }

    public override void Exit() => _motor.ResumeMovement();
}
