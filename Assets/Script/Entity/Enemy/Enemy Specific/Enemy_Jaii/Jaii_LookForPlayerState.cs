using UnityEngine;

public class Jaii_LookForPlayerState : LookForPlayerState
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

        if(isLookingForPlayerDone)
        {
            if(isPlayerMinRange) 
                stateMachine.ChangeState(jaii.PlayerDetectedState);
            else 
                stateMachine.ChangeState(jaii.MoveState);
        }
    }
}
