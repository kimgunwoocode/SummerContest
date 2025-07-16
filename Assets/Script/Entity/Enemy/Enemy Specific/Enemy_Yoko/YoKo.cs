using UnityEngine;

public class YoKo : EnemyEntity
{
    [Header("Yoko Details")]
    public Transform player; // TODO: 나중에 게임매니저 이용

    [Header("States")]
    [SerializeField] Yoko_IdleState idleState;
    [SerializeField] Yoko_MoveState moveState;
    [SerializeField] Yoko_PlayerDetected playerDetectedState;
    [SerializeField] Yoko_JumpState jumpState;
    [SerializeField] Yoko_LookForPlayerState lookForPlayerState;
    [SerializeField] Yoko_MeleeAttackState meleeAttackState;
    [SerializeField] Yoko_KnockbackState knockbackState;
    [SerializeField] Yoko_DeadState deadState;

    public Yoko_IdleState IdleState => idleState;
    public Yoko_MoveState MoveState => moveState;
    public Yoko_PlayerDetected PlayerDetectedState => playerDetectedState;
    public Yoko_JumpState JumpState => jumpState;
    public Yoko_LookForPlayerState LookForPlayerState => lookForPlayerState;
    public Yoko_MeleeAttackState MeleeAttackState => meleeAttackState;
    public Yoko_KnockbackState KnockbackState => knockbackState;
    public Yoko_DeadState DeadState => deadState;

    protected override void Awake()
    {
        base.Awake();

        idleState.Initialize(this, stateMachine);
        moveState.Initialize(this, stateMachine);
        playerDetectedState.Initialize(this, stateMachine);
        jumpState.Initialize(this, stateMachine);
        lookForPlayerState.Initialize(this, stateMachine);
        meleeAttackState.Initialize(this, stateMachine);
        knockbackState.Initialize(this, stateMachine);
        deadState.Initialize(this, stateMachine);

        stateMachine.Initialize(moveState);
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