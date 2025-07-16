using UnityEngine;

public class MoveState : State
{
    [SerializeField] protected float moveSpeed = 3;

    protected bool isLedge;
    protected bool isGround;
    protected bool isWall;
    protected bool isPlayerMinRange;
    protected bool isPlayerBehind;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "move";
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isLedge = enemy.ChackLedge();
        isGround = enemy.CheckGround();
        isWall = enemy.CheckWall();
        isPlayerMinRange = enemy.CheckPlayerMinRange();
        isPlayerBehind = enemy.CheckPlayerBehind();
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(moveSpeed);
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
