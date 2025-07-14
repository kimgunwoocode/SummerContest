using UnityEngine;

public class MoveState : State
{
    protected D_MoveState stateData;

    protected bool isLedge;
    protected bool isGround;
    protected bool isWall;
    protected bool isPlayerMinRange;
    protected bool isPlayerBehind;
    

    public MoveState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isLedge = enemy.ChackLedge();
        isGround = enemy.CheckGround();
        isWall = enemy.ChackWall();
        isPlayerMinRange = enemy.CheckPlayerMinRange();
        isPlayerBehind = enemy.CheckPlayerBehind();
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(stateData.moveSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
       
    }
}
