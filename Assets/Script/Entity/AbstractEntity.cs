using UnityEngine;

public abstract class AbstractEntity : MonoBehaviour
{
    [Header("Entity's life info")]
    [SerializeField] protected int maxHP;
    [SerializeField] protected int currentHP;
    [SerializeField] protected float attackRate;

    protected bool facingLeft = true;
    protected int facingDir = -1; // Left:-1, Right:1

    [Header("Wall Check")]
    [SerializeField] protected float wallCheckDistance;
    // [SerializeField] protected Transform wallCheckerTransform;
    // [SerializeField] protected LayerMask wallLayer;
    protected bool isWall;

    [Header("Attack")]
    [SerializeField] Transform attackCheck;
    [SerializeField] float attackCheckRadius;

    [Header("Knockback")]
    [SerializeField] protected Vector2 knockbackForce;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnockBack;

    public abstract void Attack();

    /// <summary>
    /// 공격을 맞았을 때 호출되는 함수
    /// </summary>
    /// <param name="damage">입는 데미지 양</param>
    /// <param name="hitDir">공격 방향 (1: 오른쪽, -1: 왼쪽)</param>
    public abstract void TakeDamage(int damage, int hitDir);
    protected abstract void Move();
    protected abstract void Die();

}
