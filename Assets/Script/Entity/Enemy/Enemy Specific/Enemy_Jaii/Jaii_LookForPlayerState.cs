using UnityEngine;

public class Jaii_LookForPlayerState : LookForPlayerState
{
  Jaii jaii;

    public Jaii_LookForPlayerState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayerState stateData, Jaii jaii) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.jaii = jaii;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isLookingForPlayerDone)
        {
            if(isPlayerMinRange) 
                stateMachine.ChangeState(jaii.playerDetectedState);
            else 
                stateMachine.ChangeState(jaii.moveState);
        }
    }
}
