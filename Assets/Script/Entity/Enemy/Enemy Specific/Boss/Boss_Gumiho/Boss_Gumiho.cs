using UnityEngine;

public class Boss_Gumiho : EnemyEntity
{
    [Header("States")]
    [SerializeField] Gumiho_MoveState moveState;
    [SerializeField] Gumiho_ClawAttackState clawAttackState;
    [SerializeField] Gumiho_TailAttackState tailAttackState;
    [SerializeField] Gumiho_KnockbackState knockbackState;
    [SerializeField] Gumiho_DeadState deadState;
 
    public Gumiho_MoveState MoveState => moveState;
    public Gumiho_ClawAttackState ClawAttackState => clawAttackState;
    public Gumiho_TailAttackState TailAttackState => tailAttackState;
    public Gumiho_KnockbackState KnockbackState => knockbackState;
    public Gumiho_DeadState DeadState => deadState;

    public bool canBeKnockedBack = true;
    public Transform player;

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
        if(!canBeKnockedBack) return;

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

    public void LookAtPlayer()
    {
        Vector2 directionToPlayer = player.position - transform.position;

        if (directionToPlayer.x > 0 && facingDir == -1)
        {
            Flip();
        }
        else if (directionToPlayer.x < 0 && facingDir == 1)
        {
            Flip();
        }
    }

    public new void OnDrawGizmos()
    {
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * enemyData.ledgeCheckDistance));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDir * enemyData.wallCheckDistance));
        
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDir * enemyData.closeRangeActionDistance), .2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, enemyData.groundCkeckRadius);
    }
}
