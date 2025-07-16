using UnityEngine;

public class Jaii_MeleeAttackState : MeleeAttackState
{
    Jaii jaii;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        jaii = enemy as Jaii;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished)
        {
            if(isPlayerMinRange)
            {
                stateMachine.ChangeState(jaii.PlayerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(jaii.LookForPlayerState);
            }
        }
    }
}
