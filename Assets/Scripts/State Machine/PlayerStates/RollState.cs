using System.Collections;
using UnityEngine;

public class RollState : State
{
    private readonly PlayerCharacter _character;
    private readonly CharacterMotor _motor;
    private static readonly int Rolling = Animator.StringToHash("Rolling");

    public RollState(PlayerCharacter character, CharacterMotor motor)
    {
        _character = character;
        _motor = motor;
    }
    
    public override void Enter()
    {
        base.Enter();
        _character.animator.SetBool(Rolling, true);
        _motor.Roll();
        _character.StartCoroutine(EndState());
    }

    private IEnumerator EndState()
    {
        yield return WaitHelper.QuarterSecond;
        AudioController.playAudioFile("Roll");
        CameraShake.shakeCamera(0.004f, 0.25f);
        yield return WaitHelper.QuarterSecond;
        _character.animator.SetBool(Rolling, false);
        _character.characterStateMachine.ChangeState(_character.standingState);
    }
}
