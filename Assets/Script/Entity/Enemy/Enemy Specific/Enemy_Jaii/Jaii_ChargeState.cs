using UnityEngine;

public class Jaii_ChargeState : ChargeState
{
    Jaii jaii;

    public Jaii_ChargeState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, Jaii jaii) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.jaii = jaii;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(performCloseRangeAction)
        {
            stateMachine.ChangeState(jaii.meleeAttackState);
        }
        else if(!isLedge)
        {
            jaii.stunState.SetDoStunKnockback(false);
            stateMachine.ChangeState(jaii.stunState);
        }
        else if(isWall)
        {
            jaii.stunState.SetDoStunKnockback(true);
            stateMachine.ChangeState(jaii.stunState);
        }
        else if(isChargeTimeOver)
        {
            jaii.stunState.SetDoStunKnockback(false);
            stateMachine.ChangeState(jaii.stunState);
        }
    }
}
