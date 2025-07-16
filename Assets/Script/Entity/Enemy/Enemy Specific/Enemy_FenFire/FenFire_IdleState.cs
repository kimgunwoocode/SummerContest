using UnityEngine;

public class FenFire_IdleState : IdleState
{
    FenFire fenFire;
    
    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        fenFire = enemy as FenFire;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isIdleTimeOver)
        {
            stateMachine.ChangeState(fenFire.AttackState);
        }
    }
}
