using UnityEngine;

public abstract class AbstractEntity : MonoBehaviour
{
    [Header("Entity's life info")]
    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float attackRate;

    [Header("Movement System")]
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;
    [SerializeField] protected bool facingLeft;
    [SerializeField] protected int facingDir; // Left:-1, Right:1

    [Header("Ground Check")]
    [SerializeField] protected float groundCheckDistance = 0.1f;
    [SerializeField] protected Transform groundCheckerTransform;
    [SerializeField] protected LayerMask groundLayer;
    protected bool isGrounded;

    [Header("Wall Check")]
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform wallCheckerTransform;
    [SerializeField] protected LayerMask wallLayer;
    protected bool isWall;

    [Header("knockback")]
    [SerializeField] protected Vector2 knockbackForce;
    [SerializeField] protected float knockbackDuration;
    [SerializeField] protected bool isknockBack;
    protected int knockbackDir; // -1 or 1

    public abstract void Attack();
    protected abstract void Move();

    // public abstract void TakeDamage(float damage);
    // protected abstract void Die();
    // protected abstract void DoKnockback();

}
