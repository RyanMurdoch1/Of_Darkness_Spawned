using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : State
{
    private readonly WaitForSeconds _lockTime = new WaitForSeconds(0.5f);
    public Vector2 damageDirection;
    private readonly CharacterController _characterController;
    private readonly CharacterMotor _characterMotor;
    private static readonly int Damaged = Animator.StringToHash("Damaged");

    public DamagedState(StateMachine stateMachine, CharacterController characterController, CharacterMotor characterMotor) : base(stateMachine)
    {
        _characterController = characterController;
        _characterMotor = characterMotor;
    }
    
    public override void Enter()
    {
        base.Enter();
        _characterController.animator.SetBool(Damaged, true);
        _characterMotor.TakeDamage(damageDirection);
        _characterController.StartCoroutine(DamageLock());
    }

    public override void Exit()
    {
        _characterController.animator.SetBool(Damaged, false);
    }

    private IEnumerator DamageLock()
    {
        CameraShake.shakeCamera(0.01f, 0.6f);
        yield return _lockTime;
        StateMachine.ChangeState(_characterController.standingState);
    }
}
