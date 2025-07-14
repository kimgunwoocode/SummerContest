using UnityEngine;

public class Jaii_MoveState : MoveState
{
    Jaii jaii;

    public Jaii_MoveState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Jaii jaii) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.jaii = jaii;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isPlayerMinRange)
        {
            stateMachine.ChangeState(jaii.playerDetectedState);
        }
        else if ((!isLedge && isGround) || isWall)
        {
            enemy.Flip();
        }
        else if (isPlayerBehind)
        {
            stateMachine.ChangeState(jaii.lookForPlayerState);
        }
    }
}
