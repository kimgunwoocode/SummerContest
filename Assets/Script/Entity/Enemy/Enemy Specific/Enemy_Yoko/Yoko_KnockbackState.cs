using UnityEngine;

public class Yoko_KnockbackState : KnockbackState
{
    YoKo yoKo;

    public Yoko_KnockbackState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_KnockbackState stateData, YoKo yoKo) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.yoKo = yoKo;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isKnockbackOver)
        {
            if(performCloseRangeAction)
            {
                stateMachine.ChangeState(yoKo.meleeAttackState);
            }
            else if(isPlayerMinRange)
            {
                stateMachine.ChangeState(yoKo.jumpState);
            }
            else
            {
                stateMachine.ChangeState(yoKo.lookForPlayerState);
            }
        }
    }
}
