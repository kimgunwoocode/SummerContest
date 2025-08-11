using UnityEngine;

public class Gapsangwi_Ground_DeadState : DeadState
{
    Gapsangwi_Ground gapsangwi_Ground;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        gapsangwi_Ground = enemy as Gapsangwi_Ground;
    }
}
