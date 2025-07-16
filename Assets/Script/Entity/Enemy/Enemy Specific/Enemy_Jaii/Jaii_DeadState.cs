using UnityEngine;

public class Jaii_DeadState : DeadState
{
    Jaii jaii;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        jaii = enemy as Jaii;
    }
}
