using UnityEngine;

public class ChargeState : State
{
    [SerializeField, Tooltip("돌진 속도")]
    protected float chargeSpeed = 6f;
    
    [SerializeField, Tooltip("돌진 지속 시간")]
    protected float chargeTime = 2f;

    protected bool isLedge;
    protected bool isWall;

    protected bool isPlayerMinRange;
    protected bool performCloseRangeAction;

    protected bool isChargeTimeOver;

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
