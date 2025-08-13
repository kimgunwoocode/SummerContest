using UnityEngine;

public class Gumiho_ClawAttackState : MeleeAttackState
{
    Boss_Gumiho gumiho;

    [SerializeField] float attackCooldown;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "clawAttack";
        gumiho = enemy as Boss_Gumiho;   
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        gumiho.IdleState.SetIdleTime(attackCooldown);

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(gumiho.IdleState);
        }
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}
