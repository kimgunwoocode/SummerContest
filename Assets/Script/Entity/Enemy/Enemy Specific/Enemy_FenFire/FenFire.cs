using UnityEngine;

public class FenFire : EnemyEntity
{
    [Header("States")]
    [SerializeField] FenFire_IdleState idleState;
    [SerializeField] FenFire_AttackState attackState;
    [SerializeField] FenFire_KnockbackState knockbackState;
    [SerializeField] FenFire_DeadState deadState;
    
    public FenFire_IdleState IdleState => idleState;
    public FenFire_AttackState AttackState => attackState;
    public FenFire_KnockbackState KnockbackState => knockbackState;
    public FenFire_DeadState DeadState => deadState;

    protected override void Awake()
    {
        base.Awake();

        idleState.Initialize(this, stateMachine);
        attackState.Initialize(this, stateMachine);
        knockbackState.Initialize(this, stateMachine);
        deadState.Initialize(this, stateMachine);

        stateMachine.Initialize(idleState);
    }

    public override void TakeDamage(int damageAmount, Vector2 attackerPosition)
    {
        base.TakeDamage(damageAmount, attackerPosition);

        if(isDead && stateMachine.currentState != deadState)
        {
            stateMachine.ChangeState(deadState);
        }
        else
        {
            if(stateMachine.currentState != knockbackState)
            {
                stateMachine.ChangeState(knockbackState);
            }
        }
    }
}