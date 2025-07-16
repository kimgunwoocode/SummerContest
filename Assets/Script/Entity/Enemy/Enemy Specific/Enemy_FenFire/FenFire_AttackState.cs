using UnityEngine;

public class FenFire_AttackState : AttackState
{
    FenFire fenFire;

    [Header("Attack Details")]
    [SerializeField] Enemy_Bullet bulletPrefab;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float bulletLifetime = 1f;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        fenFire = enemy as FenFire;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished)
        {
            stateMachine.ChangeState(fenFire.IdleState);
        }
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        
        CreatBullet();
    }

    void CreatBullet()
    {
        Enemy_Bullet newBullet = Instantiate(bulletPrefab, attackPosition.position, Quaternion.identity);

        Vector2 bulletVelocity = new Vector2(bulletSpeed, 0);
        newBullet.SetVelocity(bulletVelocity * enemy.facingDir);

        Destroy(newBullet.gameObject, bulletLifetime);
    }
}