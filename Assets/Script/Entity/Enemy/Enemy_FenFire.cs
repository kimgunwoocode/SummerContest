using Unity.Mathematics;
using UnityEngine;

public class Enemy_FenFire : Enemy
{
    [Header("FenFire Details")]
    [SerializeField] Enemy_Bullet bulletPrefab;
    [SerializeField] Transform bulletPoint;
    [SerializeField] float bulletSpeed = 8f;
    [SerializeField] float bulletLifetime = 5f;
 
    float lastTimeAttacked;


    protected override void Update()
    {
        base.Update();

        bool canAttack = Time.time > lastTimeAttacked + attackRate;

        if(canAttack) Attack();
    }

    public override void Attack()
    {
        base.Attack();
        lastTimeAttacked = Time.time;
        CreatBullet(); // 나중에 애니매이션에서 호출하도록 수정
    }

    void CreatBullet()
    {
        Enemy_Bullet newBullet = Instantiate(bulletPrefab, bulletPoint.position, Quaternion.identity);

        Vector2 bulletVelocity = new Vector2(bulletSpeed, 0);
        newBullet.SetVelocity(bulletVelocity * facingDir);

        Destroy(newBullet.gameObject, bulletLifetime);
    }

    
}
