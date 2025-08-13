using Unity.VisualScripting;
using UnityEngine;

public class Gumiho_KnockbackState : KnockbackState
{
    Boss_Gumiho gumiho;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        gumiho = enemy as Boss_Gumiho;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isKnockbackOver)
        {
            if(gumiho.currentHP == gumiho.phase2HP) // Phase 2
            {
                stateMachine.ChangeState(gumiho.Phase2State);
            }
            else if (gumiho.MoveState.isClawAttackCancelled) // 발톱 할퀴기가 경직으로 캔슬됐을 경우
            {
                gumiho.IdleState.SetIdleTime(0f);
                stateMachine.ChangeState(gumiho.MoveState);
            }
            else
            {
                stateMachine.ChangeState(gumiho.IdleState);
            }
        }
    }
}