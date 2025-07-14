using UnityEditor;
using UnityEngine;

public class Jaii_PlayerDetectedState : PlayerDetectedState
{
    Jaii jaii;

    public Jaii_PlayerDetectedState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData, Jaii jaii) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.jaii = jaii;
    }
    
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(performCloseRangeAction)
        {
            stateMachine.ChangeState(jaii.meleeAttackState);
        }
        else if(performLongRangeAction)
        {
            stateMachine.ChangeState(jaii.chargeState);
        }
        else if(!isPlayerMaxRange)
        {
            stateMachine.ChangeState(jaii.lookForPlayerState);
        }
        else if(!isLedge)
        {
            enemy.Flip();
            stateMachine.ChangeState(jaii.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
