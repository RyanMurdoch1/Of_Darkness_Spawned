using System.Collections;
using UnityEngine;

public class RollState : State
{
    private readonly PlayerCharacter _character;
    private static readonly int Rolling = Animator.StringToHash("Rolling");

    public RollState(PlayerCharacter character)
    {
        _character = character;
    }

    public override void Enter()
    {
        base.Enter();
        _character.animator.SetBool(Rolling, true);
        _character.characterMotor.Roll();
        _character.StartCoroutine(EndState());
    }

    private IEnumerator EndState()
    {
        yield return WaitHelper.QuarterSecond;
        AudioController.playAudioFile("Roll");
        CameraShake.shakeCamera();
        yield return WaitHelper.QuarterSecond;
        _character.animator.SetBool(Rolling, false);
        _character.characterStateMachine.ChangeState(_character.standingState);
    }
}
