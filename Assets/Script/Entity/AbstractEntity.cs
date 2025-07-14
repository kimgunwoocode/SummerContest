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
    protected abstract void Move();

}
