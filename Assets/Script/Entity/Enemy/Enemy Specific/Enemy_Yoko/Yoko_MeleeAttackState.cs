using UnityEngine;

public class Yoko_MeleeAttackState : MeleeAttackState
{
    YoKo yoKo;
    public Yoko_MeleeAttackState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData, YoKo yoKo) : base(enemy, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.yoKo = yoKo;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished)
        {
            if(isPlayerMinRange)
            {
                stateMachine.ChangeState(yoKo.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(yoKo.lookForPlayerState);
            }
        }
    }
}
