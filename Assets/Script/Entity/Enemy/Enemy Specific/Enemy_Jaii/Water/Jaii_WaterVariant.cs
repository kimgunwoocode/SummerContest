using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jaii_WaterVariant : EnemyEntity
{
    [SerializeField] protected Transform ceilingCheck;

    [Header("States")]
    [SerializeField] Jaii_MoveState moveState;
    [SerializeField] Jaii_PlayerDetectedState playerDetectedState;
    [SerializeField] Jaii_LookForPlayerState lookForPlayerState;
    [SerializeField] Jaii_WaterVariant_ChargeState chargeState;
    [SerializeField] Jaii_MeleeAttackState meleeAttackState;
    [SerializeField] Jaii_KnockbackState knockbackState;
    [SerializeField] Jaii_StunState stunState;
    [SerializeField] Jaii_DeadState deadState;

    public Jaii_MoveState MoveState => moveState;
    public Jaii_PlayerDetectedState PlayerDetectedState => playerDetectedState;
    public Jaii_LookForPlayerState LookForPlayerState => lookForPlayerState;
    public Jaii_WaterVariant_ChargeState ChargeState => chargeState;
    public Jaii_MeleeAttackState MeleeAttackState => meleeAttackState;
    public Jaii_KnockbackState KnockbackState => knockbackState;
    public Jaii_StunState StunState => stunState;
    public Jaii_DeadState DeadState => deadState;

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

    public bool CheckPlayerAbove()
    {
        return Physics2D.Raycast(playerCheck.position, -aliveGO.transform.up, enemyData.maxPlayerCheckDistance, enemyData.playerLayer);
    }

    public virtual bool CheckCeiling()
    {
        LayerMask combinedGround = enemyData.groundLayer | enemyData.platformLayer | enemyData.climbWallLayer;

        return Physics2D.OverlapCircle(ceilingCheck.position, enemyData.groundCkeckRadius, combinedGround);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(ceilingCheck.position, enemyData.groundCkeckRadius);

        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.up * enemyData.maxPlayerCheckDistance), .2f);
    }
}