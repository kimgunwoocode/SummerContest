using UnityEngine;

public class Jaii_WaterVariant_ChargeState : ChargeState
{
    Jaii_WaterVariant jaii;

    protected bool isCeiling;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        jaii = enemy as Jaii_WaterVariant;
    }

    public new void DoCheck()
    {
        isCeiling = jaii.CheckCeiling();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(jaii.MeleeAttackState);
        }
        else if (isCeiling)
        {
            jaii.StunState.SetDoStunKnockback(true);
            stateMachine.ChangeState(jaii.StunState);
        }
        else if (isChargeTimeOver)
        {
            jaii.StunState.SetDoStunKnockback(false);
            stateMachine.ChangeState(jaii.StunState);
        }
    }
}
