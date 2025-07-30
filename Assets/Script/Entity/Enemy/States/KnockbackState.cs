using UnityEngine;

public class KnockbackState : State
{
    [SerializeField] protected float knockbackPower = 3f;

    [SerializeField, Tooltip("넉백 방향 각도. x는 수평, y는 수직 방향으로 넉백이 적용됩니다.")]
    protected Vector2 knockbackAngle = new Vector2(2f, 4f);

    protected bool isKnockbackOver;
    protected bool isGround;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerMinRange;

    protected float knockbackTime = 0.2f;

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
        enemy.SetVelocity(knockbackPower, knockbackAngle, enemy.lastDamageDirection);
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
