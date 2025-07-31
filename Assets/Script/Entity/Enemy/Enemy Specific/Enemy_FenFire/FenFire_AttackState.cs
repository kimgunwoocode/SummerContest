using UnityEngine;

public class FenFire_AttackState : AttackState
{
    FenFire fenFire;

    [Header("Attack Details")]
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float bulletLifetime = 1f;

    private Enemy_Bullet bullet;
    private Vector2 bulletVelocity;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        fenFire = enemy as FenFire;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(fenFire.IdleState);
        }
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        CreatBullet();
    }

    private void CreatBullet()
    {
        bullet = PoolManager.instance.Get(0).GetComponent<Enemy_Bullet>();
        bullet.gameObject.transform.position = attackPosition.position;

        bulletVelocity = new Vector2(bulletSpeed, 0);
        bullet.SetVelocity(bulletVelocity * enemy.facingDir);

        Invoke(nameof(DestroyBullet), bulletLifetime);
    }

    private void DestroyBullet()
    {
        bullet.gameObject.SetActive(false);
    }
}