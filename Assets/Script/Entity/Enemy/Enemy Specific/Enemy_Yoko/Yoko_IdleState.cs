using UnityEngine;

public class Yoko_IdleState : IdleState
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
        else if (isPlayerBehind)
        {
            stateMachine.ChangeState(yoKo.LookForPlayerState);
        }
        else if(isIdleTimeOver)
        {
            stateMachine.ChangeState(yoKo.MoveState);
        }
    }
}