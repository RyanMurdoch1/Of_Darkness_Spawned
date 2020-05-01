﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : State
{
    private readonly WaitForSeconds _lockTime = new WaitForSeconds(0.5f);
    public Vector2 damageDirection;
    private readonly CharacterController _character;
    private static readonly int Damaged = Animator.StringToHash("Damaged");

    public DamagedState(CharacterController character)
    {
        _character = character;
    }
    
    public override void Enter()
    {
        base.Enter();
        _character.animator.SetBool(Damaged, true);
        _character.characterMotor.TakeDamage(damageDirection);
        _character.StartCoroutine(DamageLock());
    }

    public override void Exit()
    {
        _character.animator.SetBool(Damaged, false);
    }

    private IEnumerator DamageLock()
    {
        CameraShake.shakeCamera(0.01f, 0.6f);
        yield return _lockTime;
        _character.characterStateMachine.ChangeState(_character.standingState);
    }
}