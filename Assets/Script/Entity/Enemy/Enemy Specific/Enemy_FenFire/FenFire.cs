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

    public Transform player;

    protected override void Start()
    {
        base.Start();
        
        player = Singleton.GameManager_Instance.Get<GameManager>().Player.transform;
        stateMachine.Initialize(idleState);
    }

    public override void TakeDamage(int damageAmount, Vector2 attackerPosition)
    {
        //if (stateMachine.currentState == knockbackState) return;

        base.TakeDamage(damageAmount, attackerPosition);

        if(isDead && stateMachine.currentState != deadState)
        {
            SoundManager.instance.PlaySFX("enemy_death");
            stateMachine.ChangeState(deadState);
        }
        else if(stateMachine.currentState != knockbackState)
        {
            stateMachine.ChangeState(knockbackState);
        }
    }
}