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
            SoundManager.instance.PlaySFX("jaii_attack", 0.13f, 1f);
            enemy.SetVelocity(stunKnockbackPower, stunKnockbackAngle, -enemy.facingDir);
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
                stateMachine.ChangeState(jaii.PlayerDetectedState);
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
