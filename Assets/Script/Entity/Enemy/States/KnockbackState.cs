using UnityEngine;

public class KnockbackState : State
{
    D_KnockbackState stateData;

    protected bool isKnockbackOver;
    protected bool isGround;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerMinRange;


    public KnockbackState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_KnockbackState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isGround = enemy.CheckGround();
        //performCloseRangeAction = enemy.CheckPlayerInCloseRangeAction();
        //isPlayerMinRange = enemy.CheckPlayerMinRange();
    }

    public override void Enter()
    {
        base.Enter();

        isKnockbackOver = false;
        isMovementStopped = false;
        enemy.SetVelocity(stateData.knockbackSpeed, stateData.knockbackAngle, enemy.lastDamegeDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isGround && !isMovementStopped && Time.time >= startTime + stateData.knockbackTime)
        {
            isMovementStopped = true;
            isKnockbackOver = true;
            enemy.SetVelocity(0f);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
