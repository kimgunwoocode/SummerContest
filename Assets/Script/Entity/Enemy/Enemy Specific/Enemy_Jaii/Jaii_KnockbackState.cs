using UnityEngine;

public class Jaii_KnockbackState : KnockbackState
{
    Jaii jaii;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        jaii = enemy as Jaii;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isKnockbackOver)
        {
            if(performCloseRangeAction)
            {
                stateMachine.ChangeState(jaii.MeleeAttackState);
            }
            else if(isPlayerMinRange)
            {
                stateMachine.ChangeState(jaii.ChargeState);
            }
            else
            {
                stateMachine.ChangeState(jaii.LookForPlayerState);
            }
        }
    }
}
