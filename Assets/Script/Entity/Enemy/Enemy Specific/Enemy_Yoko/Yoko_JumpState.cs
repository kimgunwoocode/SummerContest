using System;
using Unity.VisualScripting;
using UnityEngine;

public class Yoko_JumpState : JumpState
{
    YoKo yoKo;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        yoKo = enemy as YoKo;
    }

    public override void Enter()
    {
        base.Enter();

        // TODO: 포물선 연산 시뮬레이션 활용하도록 수정
        float distanceFromPlayer = Math.Abs(yoKo.player.position.x - enemy.aliveGO.transform.position.x) - jumpOffset;
        enemy.rb.AddForce(new Vector2(distanceFromPlayer * enemy.facingDir, jumpHeight), ForceMode2D.Impulse);
    }
    
    protected override void OnJumpLanding()
    {
        base.OnJumpLanding();

        if(performCloseRangeAction)
        {
            stateMachine.ChangeState(yoKo.MeleeAttackState);
        }
        else if(!isLedge || isWall)
        {
            stateMachine.ChangeState(yoKo.LookForPlayerState);
        }
        else if(isJumpDone)
        {
            if(isPlayerMinRange)
            {
                stateMachine.ChangeState(yoKo.PlayerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(yoKo.LookForPlayerState);
            }
        }
    }
}
