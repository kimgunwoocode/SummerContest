using UnityEngine;

public class Gumiho_TailAttackState : MeleeAttackState
{
    Boss_Gumiho gumiho;

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
            stateMachine.ChangeState(gumiho.MoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        gumiho.canBeKnockedBack = true;
    }
}
