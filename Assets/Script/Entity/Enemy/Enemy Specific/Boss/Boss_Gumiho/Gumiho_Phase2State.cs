using UnityEngine;

public class Gumiho_Phase2State : Boss_PhaseChangeState
{
    Boss_Gumiho gumiho;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "phase2";
        gumiho = enemy as Boss_Gumiho;
    }

    public override void Enter()
    {
        base.Enter();

        // 페이즈 전환 효과
        // 플레이어 밀치기
        // 파워 업 파티클 재생
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isPhaseChangeTimeOver)
        {
            stateMachine.ChangeState(gumiho.MoveState);
        }
    }
}
