using UnityEngine;

/// <summary>
/// 적이 뒤에 있는 플레이어를 감지하고, 플레이어를 향해 돌아서는 상태.
/// </summary>
public class LookForPlayerState : State
{
    [SerializeField, Tooltip("플레이어를 감지하고 발견 상태로 전환하기까지의 딜레이 시간.")]
    protected float delayTime = 0.2f;

    protected bool isPlayerMinRange;
    protected bool isPlayerBehind;

    protected bool isLookingForPlayerDone;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "lookForPlayer";
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isPlayerMinRange = enemy.CheckPlayerMinRange();
        isPlayerBehind = enemy.CheckPlayerBehind();
    }

    public override void Enter()
    {
        base.Enter();
        isLookingForPlayerDone = false;
        enemy.SetVelocity(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + delayTime)
        {
            if(isPlayerBehind)
            {
                enemy.Flip();
            }
            isLookingForPlayerDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
