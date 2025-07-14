using UnityEngine;

public class Yoko_PlayerDetected : PlayerDetectedState
{
    YoKo yoKo;

    public Yoko_PlayerDetected(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData, YoKo yoKo) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.yoKo = yoKo;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(performCloseRangeAction)
        {
            stateMachine.ChangeState(yoKo.meleeAttackState);
        }
        else if(performLongRangeAction)
        {
            stateMachine.ChangeState(yoKo.jumpState);
        }
        else if(!isPlayerMaxRange)
        {
            stateMachine.ChangeState(yoKo.lookForPlayerState);
        }
        else if(!isLedge)
        {
            enemy.Flip();
            stateMachine.ChangeState(yoKo.moveState);
        }
    }
}
