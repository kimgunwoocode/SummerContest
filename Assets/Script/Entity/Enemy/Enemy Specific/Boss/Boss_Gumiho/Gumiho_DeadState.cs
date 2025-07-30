using UnityEngine;

public class Gumiho_DeadState : DeadState
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

        // 여기에 아이템 획득 등의 함수 추가
    }
}
