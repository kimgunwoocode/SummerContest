using UnityEngine;

public class Jaii_MoveState : MoveState
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

        if(isPlayerMinRange)
        {
            stateMachine.ChangeState(jaii.PlayerDetectedState);
        }
        else if ((!isLedge && isGround) || isWall)
        {
            enemy.Flip();
        }
        else if (isPlayerBehind)
        {
            stateMachine.ChangeState(jaii.LookForPlayerState);
        }
    }
}
