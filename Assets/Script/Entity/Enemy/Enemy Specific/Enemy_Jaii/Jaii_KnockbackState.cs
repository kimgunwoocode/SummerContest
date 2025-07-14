using UnityEngine;

public class Jaii_KnockbackState : KnockbackState
{
    Jaii jaii;

    public Jaii_KnockbackState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_KnockbackState stateData, Jaii jaii) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.jaii = jaii;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isKnockbackOver)
        {
            if(performCloseRangeAction)
            {
                stateMachine.ChangeState(jaii.meleeAttackState);
            }
            else if(isPlayerMinRange)
            {
                stateMachine.ChangeState(jaii.chargeState);
            }
            else
            {
                stateMachine.ChangeState(jaii.lookForPlayerState);
            }
        }
    }
}
