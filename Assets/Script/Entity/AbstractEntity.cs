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

    [Header("knockback")]
    [SerializeField] protected Vector2 knockbackForce;
    [SerializeField] protected float knockbackDuration;
    [SerializeField] protected bool isknockBack;
    protected int knockbackDir; // -1 or 1

    public abstract void Attack();
    protected abstract void Move();

    // public abstract void TakeDamage(int damage);
    // protected abstract void Die();
    // protected abstract void DoKnockback();

}
