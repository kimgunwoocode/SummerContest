using UnityEngine;
using UnityEngine.Events;

public class Boss_IdleState : State
{
    public UnityEvent StartBossEvent;

    protected bool isIdleTimeOver;

    public float idleTime = 0f;
    private bool hasPlayerEnteredBossRoom = false;
    private bool isPlayerInRange;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "idle";
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isPlayerInRange = enemy.CheckPlayerMaxRange() || enemy.CheckPlayerBehind();
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

        idleTime -= Time.deltaTime;
        
        if (!hasPlayerEnteredBossRoom)
        {
            if (isPlayerInRange)
            {
                hasPlayerEnteredBossRoom = true;
                startTime = Time.time;
                StartBossEvent?.Invoke();
            }
            else
            {
                return; // 최초 플레이어 인식 전
            }
        }

        if (idleTime <= 0)
        {
            isIdleTimeOver = true;
        }
    }

    public void SetIdleTime(float idleTime)
    {
        this.idleTime = idleTime;
    }

    public override void Exit()
    {
        base.Exit();

        if(isIdleTimeOver) idleTime = 0f;
    }
}