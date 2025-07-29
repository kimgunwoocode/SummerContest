using UnityEngine;

public class Boss_Gumiho : EnemyEntity
{
    [Header("States")]
    [SerializeField] private Gumiho_IdleState idleState;
    [SerializeField] private Gumiho_MoveState moveState;
    [SerializeField] private Gumiho_ChargeState chargeState;
    [SerializeField] private Gumiho_ClawAttackState clawAttackState;
    [SerializeField] private Gumiho_TailAttackState tailAttackState;
    [SerializeField] private Gumiho_FoxOrbAttackState foxOrbAttackState;
    [SerializeField] private Gumiho_FoxFireAttackState foxFireAttackState;
    [SerializeField] private Gumiho_SpiritLeapAttackState spiritLeapAttackState;
    [SerializeField] private Gumiho_KnockbackState knockbackState;
    [SerializeField] private Gumiho_DeadState deadState;

    [Header("Gumiho Details")]
    public int phase2HP = 5;
    public bool canBeKnockedBack = true;

    public Gumiho_IdleState IdleState => idleState;
    public Gumiho_MoveState MoveState => moveState;
    public Gumiho_ChargeState ChargeState => chargeState;
    public Gumiho_ClawAttackState ClawAttackState => clawAttackState;
    public Gumiho_TailAttackState TailAttackState => tailAttackState;
    public Gumiho_FoxOrbAttackState FoxOrbAttackState => foxOrbAttackState;
    public Gumiho_FoxFireAttackState FoxFireAttackState => foxFireAttackState;
    public Gumiho_SpiritLeapAttackState SpiritLeapAttackState => spiritLeapAttackState;
    public Gumiho_KnockbackState KnockbackState => knockbackState;
    public Gumiho_DeadState DeadState => deadState;

    public Transform player {get; private set;}

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(moveState);
        player = Singleton.GameManager_Instance.Get<GameManager>().Player.transform;
        canBeKnockedBack = true;
    }

    public override void TakeDamage(int damageAmount, Vector2 attackerPosition)
    {
        // 꼬리치기, 원거리 공격 시 캔슬 불가
        if (stateMachine.currentState == knockbackState || !canBeKnockedBack) return;

        base.TakeDamage(damageAmount, attackerPosition);

        // 발톱공격 중 데미지 입었는지 체크
        if (stateMachine.currentState == clawAttackState)
        {
            Debug.Log("Claw Attack Cancelled");
            moveState.isClawAttackCancelled = true;
        }

        if (isDead && stateMachine.currentState != deadState)
        {
            stateMachine.ChangeState(deadState);
        }
        else if (stateMachine.currentState != knockbackState)
        {
            stateMachine.ChangeState(knockbackState);
        }
    }

    public void LookAtPlayer()
    {
        Vector2 directionToPlayer = player.position - rb.transform.position;

        if (directionToPlayer.x > 0 && facingDir == -1 || directionToPlayer.x < 0 && facingDir == 1)
        {
            Flip();
        }
    }

    public new void OnDrawGizmos()
    {
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * enemyData.ledgeCheckDistance));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDir * enemyData.wallCheckDistance));
        
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDir * enemyData.closeRangeActionDistance), .2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDir * enemyData.minPlayerCheckDistance), .2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDir * enemyData.maxPlayerCheckDistance), .2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, enemyData.groundCkeckRadius);
    }
}