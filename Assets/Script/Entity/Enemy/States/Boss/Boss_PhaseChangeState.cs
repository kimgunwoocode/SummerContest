using UnityEngine;

public class Boss_PhaseChangeState : State
{
    [SerializeField] private float phaseChangeTime = 0f;

    protected bool isPhaseChangeTimeOver;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "phaseChange";
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0f);

        // 페이즈 전환 효과
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + phaseChangeTime)
        {
            isPhaseChangeTimeOver = true;
        }
    }
}
