using UnityEngine;

public class Gumiho_DeadState : DeadState
{
    Boss_Gumiho gumiho;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        gumiho = enemy as Boss_Gumiho;
    }
}
