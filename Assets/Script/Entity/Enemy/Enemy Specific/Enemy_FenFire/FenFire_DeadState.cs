using TreeEditor;
using UnityEngine;

public class FenFire_DeadState : DeadState
{
    FenFire fenFire;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        fenFire = enemy as FenFire;
    }
}