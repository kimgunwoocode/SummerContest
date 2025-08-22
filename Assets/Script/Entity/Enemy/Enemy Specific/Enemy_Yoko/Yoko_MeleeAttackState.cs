using UnityEngine;

public class Yoko_MeleeAttackState : MeleeAttackState
{
    YoKo yoKo;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        yoKo = enemy as YoKo;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (isPlayerMinRange)
            {
                stateMachine.ChangeState(yoKo.PlayerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(yoKo.LookForPlayerState);
            }
        }
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        SoundManager.instance.PlaySFX("yoko_attack");
    }
}
