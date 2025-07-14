using System;
using Unity.VisualScripting;
using UnityEngine;

public class Yoko_JumpState : JumpState
{
    YoKo yoKo;

    public Yoko_JumpState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_JumpState stateData, YoKo yoKo) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.yoKo = yoKo;
    }

    public override void Enter()
    {
        base.Enter();

        float distanceFromPlayer =  Math.Abs(yoKo.player.position.x - enemy.aliveGO.transform.position.x) - stateData.jumpOffset;
        enemy.rb.AddForce(new Vector2(distanceFromPlayer * enemy.facingDir, stateData.jumpHeight), ForceMode2D.Impulse);
    }
    
    protected override void OnJumpLanding()
    {
        base.OnJumpLanding();

        if(performCloseRangeAction)
        {
            stateMachine.ChangeState(yoKo.meleeAttackState);
        }
        else if(!isLedge || isWall)
        {
            stateMachine.ChangeState(yoKo.lookForPlayerState);
        }
        else if(isJumpDone)
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
