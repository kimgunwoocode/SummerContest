using UnityEngine;

public class Jaii_ChargeState : ChargeState
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

        if(performCloseRangeAction)
        {
            stateMachine.ChangeState(jaii.MeleeAttackState);
        }
        else if(!isLedge)
        {
            jaii.StunState.SetDoStunKnockback(false);
            stateMachine.ChangeState(jaii.StunState);
        }
        else if(isWall)
        {
            jaii.StunState.SetDoStunKnockback(true);
            stateMachine.ChangeState(jaii.StunState);
        }
        else if(isChargeTimeOver)
        {
            jaii.StunState.SetDoStunKnockback(false);
            stateMachine.ChangeState(jaii.StunState);
        }
    }
}