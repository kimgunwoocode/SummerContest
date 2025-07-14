using UnityEngine;
using UnityEngine.InputSystem;

public class Jaii : EnemyEntity
{
    public Jaii_MoveState moveState {get; private set;}
    public Jaii_PlayerDetectedState playerDetectedState {get; private set;}
    public Jaii_LookForPlayerState lookForPlayerState {get; private set;}
    public Jaii_ChargeState chargeState {get; private set;}
    public Jaii_MeleeAttackState meleeAttackState {get; private set;}
    public Jaii_KnockbackState knockbackState {get; private set;}
    public Jaii_StunState stunState {get; private set;}
    public Jaii_DeadState deadState {get; private set;}

    [Header("State Data")]
    [SerializeField] D_MoveState moveStateData;
    [SerializeField] D_PlayerDetectedState playerDetectedData;
    [SerializeField] D_LookForPlayerState lookForPlayerStateData;
    [SerializeField] D_ChargeState chargeStateData;
    [SerializeField] D_MeleeAttackState meleeAttackStateData;
    [SerializeField] D_KnockbackState knockbackStateData;
    [SerializeField] D_StunState stunStateData;
    [SerializeField] D_DeadState deadStateData;

    [Space]
    [SerializeField] Transform meleeAttackPosition;

    bool isStunned;

    public override void Start()
    {
        base.Start();

        moveState = new Jaii_MoveState(this, stateMachine, "move", moveStateData, this);
        playerDetectedState = new Jaii_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        lookForPlayerState = new Jaii_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        chargeState = new Jaii_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        meleeAttackState = new Jaii_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        knockbackState = new Jaii_KnockbackState(this, stateMachine, "knockback", knockbackStateData, this);
        stunState = new Jaii_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new Jaii_DeadState(this, stateMachine, "dead", deadStateData, this);

        stateMachine.Initialize(moveState);
    }

    public override void Update()
    {
        base.Update();
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
