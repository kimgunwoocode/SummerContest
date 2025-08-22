using System;
using Unity.VisualScripting;
using UnityEngine;

public class Yoko_JumpState : JumpState
{
    YoKo yoKo;

    Transform player;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        yoKo = enemy as YoKo;
    }

    public override void Enter()
    {
        base.Enter();

        SoundManager.instance.PlaySFX("yoko_jump");

        player = Singleton.GameManager_Instance.Get<GameManager>().Player.transform;
        float distanceFromPlayer = Math.Abs(player.position.x - enemy.aliveGO.transform.position.x) - jumpOffset;

        JumpToTarget(new Vector2(distanceFromPlayer, 0), jumpHeight);
    }

    protected override void OnJumpLanding()
    {
        base.OnJumpLanding();

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(yoKo.MeleeAttackState);
        }
        else if (!isLedge || isWall)
        {
            stateMachine.ChangeState(yoKo.LookForPlayerState);
        }
        else if (isJumpDone)
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
    
    private void JumpToTarget(Vector2 displacement, float apexHeight)
    {
        float gravity = Mathf.Abs(Physics2D.gravity.y * enemy.rb.gravityScale);

        // 1. 정점 높이까지 걸리는 시간
        float timeToApex = Mathf.Sqrt(2 * apexHeight / gravity);

        // 2. 정점에서 목표 지점까지 걸리는 시간
        float heightAfterApex = apexHeight - displacement.y;
        float timeFromApex = Mathf.Sqrt(2 * Mathf.Max(heightAfterApex, 0.01f) / gravity); // 음수 방지

        float totalTime = timeToApex + timeFromApex;

        // 3. 초기 속도 계산
        float velocityY = gravity * timeToApex;
        float velocityX = displacement.x / totalTime * enemy.facingDir;

        // 4. 기존 속도 제거 후 점프 적용
        enemy.rb.linearVelocity = Vector2.zero;
        enemy.rb.AddForce(new Vector2(velocityX, velocityY), ForceMode2D.Impulse);
    }
}
