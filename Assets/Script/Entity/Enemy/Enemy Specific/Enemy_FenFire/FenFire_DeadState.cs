using UnityEngine;

public class FenFire_DeadState : DeadState
{
    FenFire fenFire;

    public FenFire_DeadState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData, FenFire fenFire) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.fenFire = fenFire;
    }
}
