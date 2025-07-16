using UnityEngine;

public class Yoko_MoveState : MoveState
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

        if(isPlayerMinRange)
        {
            stateMachine.ChangeState(yoKo.PlayerDetectedState);
        }
        else if (!isLedge || isWall)
        {
            yoKo.IdleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(yoKo.IdleState);
        }
        else if (isPlayerBehind)
        {
            stateMachine.ChangeState(yoKo.LookForPlayerState);
        }
    }
}
