using UnityEngine;

public class Boss_IdleState : State
{
    protected bool isIdleTimeOver;
    protected float idleTime;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "idle";
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(0f);
        isIdleTimeOver = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    public void SetIdleTime(float idleTime)
    {
        this.idleTime = idleTime;
    }
}
