using UnityEngine;

public class Jaii_ChargeState : ChargeState
{
    Jaii jaii;

    [SerializeField] Transform chargeJaii;
    [SerializeField] float rollingSpeed = 5f;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        jaii = enemy as Jaii;
    }

    public override void Enter()
    {
        base.Enter();

        SoundManager.instance.PlaySFX("jaii_charge", 0.16f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(jaii.MeleeAttackState);
        }
        else if (!isLedge)
        {
            jaii.StunState.SetDoStunKnockback(false);
            stateMachine.ChangeState(jaii.StunState);
        }
        else if (isWall)
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

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // 자이 굴러가기
        chargeJaii.Rotate(0, 0, rollingSpeed * Time.fixedDeltaTime * enemy.facingDir);
    }

    public override void Exit()
    {
        base.Exit();

        chargeJaii.rotation = Quaternion.identity;
    }
}