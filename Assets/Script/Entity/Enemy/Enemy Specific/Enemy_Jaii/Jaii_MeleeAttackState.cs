using UnityEngine;

public class Jaii_MeleeAttackState : MeleeAttackState
{
    Jaii jaii;

    public Jaii_MeleeAttackState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData, Jaii jaii) : base(enemy, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.jaii = jaii;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished)
        {
            if(isPlayerMinRange)
            {
                stateMachine.ChangeState(jaii.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(jaii.lookForPlayerState);
            }
        }
    }
}
