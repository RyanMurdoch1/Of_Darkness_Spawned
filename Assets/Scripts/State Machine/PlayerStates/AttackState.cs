using System.Collections;
using UnityEngine;

public class AttackState : State
{
    private readonly PlayerCharacter _character;
    private readonly GameObject _weaponZone;
    private static readonly int Attacking = Animator.StringToHash("Attacking");

    public AttackState(PlayerCharacter character, GameObject weaponZone)
    {
        _character = character;
        _weaponZone = weaponZone;
    }
    
    public override void Enter()
    {
        base.Enter();
        _character.characterMotor.FreezeMovement();
        _character.StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        _character.animator.SetBool(Attacking, true);
        AudioController.playAudioFile("Fire Bow");
        yield return WaitHelper.ThirdSecond;
        _weaponZone.SetActive(true);
        CameraShake.shakeCamera();
        yield return WaitHelper.TenthSecond;
        _weaponZone.SetActive(false);
        yield return WaitHelper.TenthSecond;
        _character.animator.SetBool(Attacking, false);
        _character.characterStateMachine.ChangeState(_character.standingState);
    }

    public override void Exit() => _character.characterMotor.ResumeMovement();
}
