using UnityEngine;

public class FenFire_AttackState : AttackState
{
   FenFire fenFire;

    public FenFire_AttackState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, FenFire fenFire) : base(enemy, stateMachine, animBoolName, attackPosition)
    {
        this.fenFire = fenFire;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished)
        {
            stateMachine.ChangeState(fenFire.idleState);
        }
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        
        fenFire.CreatBullet();
    }
}
