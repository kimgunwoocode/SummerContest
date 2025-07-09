using System.Text.RegularExpressions;
using UnityEngine;

public class Enemy : AbstractEntity
{
    [Header("Enemy Details")]
    [SerializeField] protected float idleDuration;
    [SerializeField] protected GameObject damageTrigger;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected bool canMove;

    [Header("Death Details")]
    [SerializeField] protected float deathImpact;
    [SerializeField] protected float deathRotationSpeed;
    protected bool isDead;
    int deathRotationDir = 1;

    
    // protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected float idleTimer;
    protected bool isFrontGrounded;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        // anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        idleTimer -= Time.deltaTime;

        if(isDead) HandleDeathRotation();
    }

    public virtual void Die()
    {
        col.enabled = false;
        damageTrigger.SetActive(false);
        // anim.SetTrigger("hit");
        rb.linearVelocity = new Vector2(rb.linearVelocityX, deathImpact);
        isDead = true;

        if (Random.Range(0, 100) < 50) deathRotationDir *= -1;
    }

    void HandleDeathRotation()
    {
        transform.Rotate(0, 0, deathRotationSpeed * deathRotationDir * Time.deltaTime);
    }

    public override void Attack() 
    {
        
    }

    protected override void Move()
    {
        
    }

    protected virtual void HandleTurn(float x)
    {
        if (x < 0 && !facingLeft || x > 0 && facingLeft)
            Turn();
    }

    protected virtual void Turn()
    {
        facingDir *= -1;
        transform.Rotate(0, 180, 0);
        facingLeft = !facingLeft;
    }

    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        isFrontGrounded = Physics2D.Raycast(groundCheckerTransform.position, Vector2.down, groundCheckDistance, groundLayer);
        isWall = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, groundLayer);
    }

    protected virtual void OnDrawGizmos() 
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x ,transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(groundCheckerTransform.position, new Vector2(groundCheckerTransform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDir), transform.position.y));
    }
}
