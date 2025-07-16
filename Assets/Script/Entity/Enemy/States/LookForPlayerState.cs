using UnityEngine;

public class LookForPlayerState : State
{
    [SerializeField] protected float delayTime = 0.2f;

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
