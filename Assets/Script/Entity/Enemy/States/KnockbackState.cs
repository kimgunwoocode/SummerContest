using UnityEngine;

public class KnockbackState : State
{
    [SerializeField] protected float knockbackTime = 0.2f;
    [SerializeField] protected float knockbackSpeed = 3f;
    [SerializeField] protected Vector2 knockbackAngle = new Vector2(2f, 4f);

    protected bool isKnockbackOver;
    protected bool isGround;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerMinRange;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "knockback";
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isGround = enemy.CheckGround();
        //performCloseRangeAction = enemy.CheckPlayerInCloseRangeAction();
        //isPlayerMinRange = enemy.CheckPlayerMinRange();
    }

    public override void Enter()
    {
        base.Enter();

        isKnockbackOver = false;
        isMovementStopped = false;
        enemy.SetVelocity(knockbackSpeed, knockbackAngle, enemy.lastDamageDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isGround && !isMovementStopped && Time.time >= startTime + knockbackTime)
        {
            isMovementStopped = true;
            isKnockbackOver = true;
            enemy.SetVelocity(0f);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
