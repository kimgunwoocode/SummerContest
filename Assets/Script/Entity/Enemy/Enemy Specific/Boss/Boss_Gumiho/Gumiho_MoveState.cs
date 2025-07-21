using UnityEngine;

public class Gumiho_MoveState : Boss_MoveState
{
    Boss_Gumiho gumiho;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        gumiho = enemy as Boss_Gumiho;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(performCloseRangeAction)
        {
            stateMachine.ChangeState(gumiho.ClawAttackState);
            //stateMachine.ChangeState(gumiho.TailAttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        gumiho.LookAtPlayer();

        Vector2 target = new Vector2(gumiho.player.position.x, enemy.rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(enemy.rb.position, target, moveSpeed * Time.fixedDeltaTime);
        enemy.rb.MovePosition(newPos);
    }
}
