using UnityEngine;

public class StunState : State
{
    protected D_StunState stateData;

    protected bool isStunTimeOver;
    protected bool isGround;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerMinRange;

    public StunState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isGround = enemy.CheckGround();
        performCloseRangeAction = enemy.CheckPlayerInCloseRangeAction();
        isPlayerMinRange = enemy.CheckPlayerMinRange();
    }

    public override void Enter()
    {
        base.Enter();

        isStunTimeOver = false;
        isMovementStopped = false;
        // enemy.SetVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle, enemy.lastDamegeDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + stateData.stunTime)
        {
            isStunTimeOver = true;
        }

        // if(isGround && Time.time >= startTime + stateData.stunKnockbackTime && !isMovementStopped)
        // {
        //     isMovementStopped = true;
        //     enemy.SetVelocity(0f);
        // }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}