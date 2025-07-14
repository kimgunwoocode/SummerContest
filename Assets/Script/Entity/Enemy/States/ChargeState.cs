using UnityEngine;

public class ChargeState : State
{
    protected D_ChargeState stateData;

    protected bool isPlayerMinRange;
    protected bool isLedge;
    protected bool isWall;
    protected bool isChargeTimeOver;
    protected bool performCloseRangeAction;

    
    public ChargeState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isPlayerMinRange = enemy.CheckPlayerMinRange();
        isLedge = enemy.ChackLedge();
        isWall = enemy.ChackWall();

        performCloseRangeAction = enemy.CheckPlayerInCloseRangeAction();
    }

    public override void Enter()
    {
        base.Enter();

        isChargeTimeOver = false;
        enemy.SetVelocity(stateData.chargeSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + stateData.chargeTime)
        {
            isChargeTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
