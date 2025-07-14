using UnityEngine;

public class Jaii_DeadState : DeadState
{
   Jaii jaii;

    public Jaii_DeadState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData, Jaii jaii) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.jaii = jaii;
    }
}
