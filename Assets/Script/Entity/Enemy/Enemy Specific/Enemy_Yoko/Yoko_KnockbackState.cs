using UnityEngine;

public class Yoko_KnockbackState : KnockbackState
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

        if(isKnockbackOver)
        {
            if(performCloseRangeAction)
            {
                stateMachine.ChangeState(yoKo.MeleeAttackState);
            }
            else if(isPlayerMinRange)
            {
                stateMachine.ChangeState(yoKo.JumpState);
            }
            else
            {
                stateMachine.ChangeState(yoKo.LookForPlayerState);
            }
        }
    }
}
