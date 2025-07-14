using UnityEngine;

public class MeleeAttackState : AttackState
{
    D_MeleeAttackState stateData;

    public MeleeAttackState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData) : base(enemy, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        Collider2D[] detectedObjs = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.playerLayer);

        foreach(Collider2D col in detectedObjs)
        {
            // 플레이어 공격
            // col.GetComponent<PlayerManager>()?.TakeDamage(1, enemy.aliveGO.transform.position);
            Debug.Log("적이 플레이어를 공격함");
        }
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }
}
