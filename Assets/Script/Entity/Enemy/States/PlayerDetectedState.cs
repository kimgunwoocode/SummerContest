using UnityEngine;

public class PlayerDetectedState : State
{
    protected D_PlayerDetectedState stateData;

    protected bool isPlayerMinRange;
    protected bool isPlayerMaxRange;
    protected bool performLongRangeAction; // 돌진 가능 여부
    protected bool performCloseRangeAction; // 근접 공격 실행 여부
    protected bool isLedge;


    public PlayerDetectedState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isPlayerMinRange = enemy.CheckPlayerMinRange();
        isPlayerMaxRange = enemy.CheckPlayerMaxRange();
        isLedge = enemy.ChackLedge();
        performCloseRangeAction = enemy.CheckPlayerInCloseRangeAction();
    }

    public override void Enter()
    {
        base.Enter();

        performLongRangeAction = false;
        enemy.SetVelocity(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + stateData.longRangeActionTime)
        {
            performLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
