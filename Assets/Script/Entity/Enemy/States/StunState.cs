using UnityEngine;

public class StunState : State
{
    [SerializeField] protected float stunTime = 3f;
    [SerializeField] protected float stunKnockbackTime = 0.2f;
    [SerializeField] protected float stunKnockbackSpeed = 20f;
    [SerializeField] protected Vector2 stunKnockbackAngle;

    protected bool isStunTimeOver;
    protected bool isGround;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerMinRange;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "stun";
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isGround = enemy.CheckGround();
        performCloseRangeAction = enemy.CheckPlayerInCloseRangeAction();
        isPlayerMinRange = enemy.CheckPlayerMinRange();
    }

    public override void Enter()
    {
        base.Enter();

        isStunTimeOver = false;
        isMovementStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + stunTime)
        {
            isStunTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}