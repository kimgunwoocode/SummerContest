using UnityEngine;

public class Gumiho_TailAttackState : MeleeAttackState
{
    Boss_Gumiho gumiho;

    [SerializeField] float attackCooldown;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "tailAttack";
        gumiho = enemy as Boss_Gumiho;
    }

    public override void Enter()
    {
        base.Enter();

        gumiho.canBeKnockedBack = false;

        enemy.SetVelocity(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished)
        {
            gumiho.IdleState.SetIdleTime(attackCooldown);
            stateMachine.ChangeState(gumiho.IdleState);
        }
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        gumiho.canBeKnockedBack = true;

        Debug.Log("꼬리 공격");
    }
}