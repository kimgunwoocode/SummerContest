using UnityEngine;

public class Gumiho_ClawAttackState : MeleeAttackState
{
    Boss_Gumiho gumiho;

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

        if(isAnimationFinished)
        {
            stateMachine.ChangeState(gumiho.MoveState);
        }
    }
}
