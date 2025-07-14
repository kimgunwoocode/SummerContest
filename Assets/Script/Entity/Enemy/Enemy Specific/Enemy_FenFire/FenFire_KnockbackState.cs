using UnityEngine;

public class FenFire_KnockbackState : KnockbackState
{
   FenFire fenFire;

    public FenFire_KnockbackState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_KnockbackState stateData, FenFire fenFire) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.fenFire = fenFire;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
         if(isKnockbackOver)
        {
            stateMachine.ChangeState(fenFire.idleState);
        }
    }
}
