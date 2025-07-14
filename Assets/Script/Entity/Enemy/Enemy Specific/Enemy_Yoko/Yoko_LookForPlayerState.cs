using UnityEngine;

public class Yoko_LookForPlayerState : LookForPlayerState
{
    YoKo yoKo;

    public Yoko_LookForPlayerState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayerState stateData, YoKo yoKo) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.yoKo = yoKo;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isLookingForPlayerDone)
        {
            if(isPlayerMinRange) 
                stateMachine.ChangeState(yoKo.playerDetectedState);
            else 
                stateMachine.ChangeState(yoKo.moveState);
        }
    }
}