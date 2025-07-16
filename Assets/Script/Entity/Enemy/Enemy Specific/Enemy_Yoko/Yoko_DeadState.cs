using UnityEngine;

public class Yoko_DeadState : DeadState
{
    YoKo yoKo;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        yoKo = enemy as YoKo;
    }
}