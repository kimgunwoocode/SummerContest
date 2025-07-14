using UnityEngine;

public class Yoko_IdleState : IdleState
{
   YoKo yoKo;

    public Yoko_IdleState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, YoKo yoKo) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.yoKo = yoKo;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isPlayerMinRange)
        {
            stateMachine.ChangeState(yoKo.playerDetectedState);
        }
        else if (isPlayerBehind)
        {
            stateMachine.ChangeState(yoKo.lookForPlayerState);
        }
        else if(isIdleTimeOver)
        {
            stateMachine.ChangeState(yoKo.moveState);
        }
    }
}
