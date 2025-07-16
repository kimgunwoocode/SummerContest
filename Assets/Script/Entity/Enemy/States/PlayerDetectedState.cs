using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerDetectedState : State
{
    [SerializeField] protected float longRangeActionTime = 1.5f;

    protected bool isPlayerMinRange;
    protected bool isPlayerMaxRange;
    protected bool performLongRangeAction; // 돌진 가능 여부
    protected bool performCloseRangeAction; // 근접 공격 실행 여부
    protected bool isLedge;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "playerDetected";
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

        if(Time.time >= startTime + longRangeActionTime)
        {
            performLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
