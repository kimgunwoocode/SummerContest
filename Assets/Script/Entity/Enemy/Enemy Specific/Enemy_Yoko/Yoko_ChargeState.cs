using UnityEngine;

public class Yoko_ChargeState : ChargeState
{
    YoKo yoKo;

    public Yoko_ChargeState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, YoKo yoKo) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.yoKo = yoKo;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(performCloseRangeAction)
        {
            stateMachine.ChangeState(yoKo.jumpState);
        }
        else if(!isLedge || isWall)
        {
            stateMachine.ChangeState(yoKo.lookForPlayerState);
        }
        else if(isChargeTimeOver)
        {
            if(isPlayerMinRange)
            {
                stateMachine.ChangeState(yoKo.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(yoKo.lookForPlayerState);
            }
        }
    }
}
