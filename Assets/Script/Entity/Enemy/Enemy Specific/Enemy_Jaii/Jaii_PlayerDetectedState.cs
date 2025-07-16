using UnityEditor;
using UnityEngine;

public class Jaii_PlayerDetectedState : PlayerDetectedState
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

        if(performCloseRangeAction)
        {
            stateMachine.ChangeState(jaii.MeleeAttackState);
        }
        else if(performLongRangeAction)
        {
            stateMachine.ChangeState(jaii.ChargeState);
        }
        else if(!isPlayerMaxRange)
        {
            stateMachine.ChangeState(jaii.LookForPlayerState);
        }
        else if(!isLedge)
        {
            enemy.Flip();
            stateMachine.ChangeState(jaii.MoveState);
        }
    }
}
