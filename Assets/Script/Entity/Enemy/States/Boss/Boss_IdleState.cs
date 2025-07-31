using UnityEngine;
using UnityEngine.Events;

public class Boss_IdleState : State
{
    public UnityEvent StartBossEvent;

    protected bool isIdleTimeOver;

    private float idleTime = 0f;
    private bool hasPlayerEnteredBossRoom = false;
    private bool isPlayerMaxRange;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "idle";
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isPlayerMaxRange = enemy.CheckPlayerMaxRange();
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
        
        if (!hasPlayerEnteredBossRoom)
        {
            if (isPlayerMaxRange)
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

        if (Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    public void SetIdleTime(float idleTime)
    {
        this.idleTime = idleTime;
    }
}