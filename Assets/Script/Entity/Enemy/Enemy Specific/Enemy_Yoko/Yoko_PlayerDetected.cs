using UnityEngine;

public class Yoko_PlayerDetected : PlayerDetectedState
{
    YoKo yoKo;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        yoKo = enemy as YoKo;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(performCloseRangeAction)
        {
            stateMachine.ChangeState(yoKo.MeleeAttackState);
        }
        else if(performLongRangeAction)
        {
            stateMachine.ChangeState(yoKo.JumpState);
        }
        else if(!isPlayerMaxRange)
        {
            stateMachine.ChangeState(yoKo.LookForPlayerState);
        }
        else if(!isLedge)
        {
            enemy.Flip();
            stateMachine.ChangeState(yoKo.MoveState);
        }
    }
}
