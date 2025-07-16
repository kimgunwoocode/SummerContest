using UnityEngine;

public class Jaii_StunState : StunState
{
    Jaii jaii;

    bool DoStunKnockback;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        jaii = enemy as Jaii;
    }

    public override void Enter()
    {
        base.Enter();

        if (DoStunKnockback)
        {
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

    public void SetDoStunKnockback(bool DoStunKnockback)
    {
        this.DoStunKnockback = DoStunKnockback;
    }
}
