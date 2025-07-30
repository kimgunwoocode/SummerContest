using UnityEngine;

public class Gumiho_IdleState : Boss_IdleState
{
    Boss_Gumiho gumiho;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        gumiho = enemy as Boss_Gumiho;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isIdleTimeOver)
        {
            stateMachine.ChangeState(gumiho.MoveState);
        }
    }
}