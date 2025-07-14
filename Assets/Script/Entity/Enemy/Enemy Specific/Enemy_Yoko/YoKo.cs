using UnityEngine;

public class YoKo : EnemyEntity
{
    public Yoko_IdleState idleState {get; private set;}
    public Yoko_MoveState moveState {get; private set;}
    public Yoko_PlayerDetected playerDetectedState {get; private set;}
    public Yoko_JumpState jumpState {get; private set;}
    public Yoko_ChargeState chargeState {get; private set;}
    public Yoko_LookForPlayerState lookForPlayerState {get; private set;}
    public Yoko_MeleeAttackState meleeAttackState {get; private set;}
    public Yoko_KnockbackState knockbackState {get; private set;}
    public Yoko_DeadState deadState {get; private set;}

    [Header("State Data")]
    [SerializeField] D_IdleState idleStateData;
    [SerializeField] D_MoveState moveStateData;
    [SerializeField] D_PlayerDetectedState playerDetectedData;
    [SerializeField] D_JumpState jumpStateData;
    [SerializeField] D_ChargeState chargeStateData;
    [SerializeField] D_LookForPlayerState lookForPlayerStateData;
    [SerializeField] D_MeleeAttackState meleeAttackStateData;
    [SerializeField] D_KnockbackState knockbackStateData;
    [SerializeField] D_DeadState deadStateData;

    [Space]
    [SerializeField] Transform meleeAttackPosition;
    public Transform player; // 나중에 게임매니저 이용

    public override void Start()
    {
        base.Start();

        // 컴포넌트로 옮기기
        idleState = new Yoko_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new Yoko_MoveState(this, stateMachine, "move", moveStateData, this);
        playerDetectedState = new Yoko_PlayerDetected(this, stateMachine, "playerDetected", playerDetectedData, this);
        jumpState = new Yoko_JumpState(this, stateMachine, "jump", jumpStateData, this);
        chargeState = new Yoko_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new Yoko_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new Yoko_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        knockbackState = new Yoko_KnockbackState(this, stateMachine, "knockback", knockbackStateData, this);
        deadState = new Yoko_DeadState(this, stateMachine, "dead", deadStateData, this);

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

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
        Gizmos.DrawWireSphere(groundCheck.position, enemyData.groundCkeckRadius);
    }
}
