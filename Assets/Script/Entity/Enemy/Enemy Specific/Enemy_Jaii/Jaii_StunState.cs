using UnityEngine;

public class Jaii_StunState : StunState
{
    Jaii jaii;

    bool DoStunKnockback;

    public Jaii_StunState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, Jaii jaii) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.jaii = jaii;
    }

    public override void Enter()
    {
        base.Enter();

        if (DoStunKnockback)
        {
            enemy.SetVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle, -enemy.facingDir);
            DoStunKnockback = false;
        }
        else
        {
            enemy.SetVelocity(0f);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isStunTimeOver)
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

    public void SetDoStunKnockback(bool DoStunKnockback)
    {
        this.DoStunKnockback = DoStunKnockback;
    }
}
