using UnityEngine;

public class LookForPlayerState : State
{
    protected D_LookForPlayerState stateData;

    protected bool isPlayerMinRange;
    protected bool isPlayerBehind;

    protected bool isLookingForPlayerDone;


    public LookForPlayerState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayerState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
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

        if(isPlayerBehind)
        {
            enemy.Flip();
        }

        if(Time.time >= startTime + stateData.delayTime)
        {
            isLookingForPlayerDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
