using UnityEngine;

public class Gapsangwi_Ground : EnemyEntity
{
    [Header("States")]
    [SerializeField] Gapsangwi_Ground_IdleState idleState;
    [SerializeField] Gapsangwi_Ground_MoveState moveState;
    [SerializeField] Gapsangwi_Ground_PlayerDetected playerDetectedState;
    [SerializeField] Gapsangwi_Ground_LookForPlayerState lookForPlayerState;
    [SerializeField] Gapsangwi_Ground_RangedAttackState rangedAttackState;
    [SerializeField] Gapsangwi_Ground_KnockbackState knockbackState;
    [SerializeField] Gapsangwi_Ground_DeadState deadState;
    
    public Gapsangwi_Ground_IdleState IdleState => idleState;
    public Gapsangwi_Ground_MoveState MoveState => moveState;
    public Gapsangwi_Ground_PlayerDetected PlayerDetectedState => playerDetectedState;
    public Gapsangwi_Ground_LookForPlayerState LookForPlayerState => lookForPlayerState;
    public Gapsangwi_Ground_RangedAttackState RangedAttackState => rangedAttackState;
    public Gapsangwi_Ground_KnockbackState KnockbackState => knockbackState;
    public Gapsangwi_Ground_DeadState DeadState => deadState;
    

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(moveState);
    }

    public override void TakeDamage(int damageAmount, Vector2 attackerPosition)
    {
        //if (stateMachine.currentState == knockbackState) return;

        base.TakeDamage(damageAmount, attackerPosition);

        if (isDead && stateMachine.currentState != deadState)
        {
            stateMachine.ChangeState(deadState);
        }
        else if (stateMachine.currentState != knockbackState)
        {
            stateMachine.ChangeState(knockbackState);
        }
    }
}
