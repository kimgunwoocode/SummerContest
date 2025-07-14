using UnityEngine;

public class FenFire_IdleState : IdleState
{
    FenFire fenFire;
    public FenFire_IdleState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, FenFire fenFire) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.fenFire = fenFire;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isIdleTimeOver)
        {
            stateMachine.ChangeState(fenFire.attackState);
        }
    }
}
