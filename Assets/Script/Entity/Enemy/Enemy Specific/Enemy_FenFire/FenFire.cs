using UnityEngine;

public class FenFire : EnemyEntity
{
    public FenFire_IdleState idleState {get; private set;}
    public FenFire_AttackState attackState {get; private set;}
    public FenFire_DeadState deadState {get; private set;}
    public FenFire_KnockbackState knockbackState {get; private set;}

    [Header("State Data")]
    [SerializeField] D_IdleState idleStateData;
    [SerializeField] D_DeadState deadStateData;
    [SerializeField] D_KnockbackState knockbackStateData;

    [Header("Attack Details")]
    [SerializeField] Transform attackPosition;
    [SerializeField] Enemy_Bullet bulletPrefab;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float bulletLifetime = 1f;

    public override void Start()
    {
        base.Start();

        idleState = new FenFire_IdleState(this, stateMachine, "idle", idleStateData, this);
        attackState = new FenFire_AttackState(this, stateMachine, "attack", attackPosition, this);
        deadState = new FenFire_DeadState(this, stateMachine, "dead", deadStateData, this);
        knockbackState = new FenFire_KnockbackState(this, stateMachine, "knockback", knockbackStateData, this);

        stateMachine.Initialize(idleState);
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

    public void CreatBullet()
    {
        Enemy_Bullet newBullet = Instantiate(bulletPrefab, attackPosition.position, Quaternion.identity);

        Vector2 bulletVelocity = new Vector2(bulletSpeed, 0);
        newBullet.SetVelocity(bulletVelocity * facingDir);

        Destroy(newBullet.gameObject, bulletLifetime);
    }

    public new void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, enemyData.groundCkeckRadius);
    }
}
