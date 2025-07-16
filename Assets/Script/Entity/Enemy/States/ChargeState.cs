using UnityEngine;

public class ChargeState : State
{
    [SerializeField] protected float chargeSpeed = 6f;
    [SerializeField] protected float chargeTime = 2f;

    protected bool isPlayerMinRange;
    protected bool isLedge;
    protected bool isWall;
    protected bool isChargeTimeOver;
    protected bool performCloseRangeAction;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "charge";
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isPlayerMinRange = enemy.CheckPlayerMinRange();
        isLedge = enemy.ChackLedge();
        isWall = enemy.CheckWall();

        performCloseRangeAction = enemy.CheckPlayerInCloseRangeAction();
    }

    public override void Enter()
    {
        base.Enter();

        isChargeTimeOver = false;
        enemy.SetVelocity(chargeSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + chargeTime)
        {
            isChargeTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
