using UnityEngine;

public class IdleState : State
{
    [SerializeField] float minIdleTime = 1f;
    [SerializeField] float maxIdleTime = 1f;

    protected bool flipAfterIdle;
    protected bool isIdleTimeOver;
    protected bool isPlayerMinRange;
    protected bool isPlayerBehind;

    protected float idleTime;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "idle";
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isPlayerMinRange = enemy.CheckPlayerMinRange();
        isPlayerBehind = enemy.CheckPlayerBehind();
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(0f);
        isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if(flipAfterIdle)
        {
            enemy.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }

    void SetRandomIdleTime()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
    }
}
