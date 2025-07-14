using UnityEngine;

public class Yoko_MoveState : MoveState
{
    YoKo yoKo;

    public Yoko_MoveState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, YoKo yoKo) : base(enemy, stateMachine, animBoolName, stateData)
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
        else if (!isLedge || isWall)
        {
            yoKo.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(yoKo.idleState);
        }
        else if (isPlayerBehind)
        {
            stateMachine.ChangeState(yoKo.lookForPlayerState);
        }
    }
}
