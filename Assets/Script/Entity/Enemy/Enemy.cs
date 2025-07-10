using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Enemy : AbstractEntity
{
    [Header("Enemy Details")]
    [SerializeField] protected float idleDuration;
    [SerializeField] protected GameObject damageTrigger;

    [Header("Player Details")]
    [SerializeField] protected Transform player;
    [SerializeField] protected LayerMask playerLayer;
    protected bool isPlayerDetected;

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
    SpriteRenderer sr => GetComponent<SpriteRenderer>();
    Color defaultColor;

    [ContextMenu("Change Facing Direction")]
    public void FlipDefaultFacingDir()
    {
        sr.flipX = !sr.flipX;
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        // anim = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        currentHP = maxHP;
        defaultColor = sr.color;

        if (sr.flipX && facingLeft)
        {
            sr.flipX = false;
            Turn();
        }

        InvokeRepeating(nameof(UpdatePlayerRef), 0, 1);
    }

    protected virtual void Update()
    {
        if (isDead)
        {
            HandleDeathRotation();
            return;
        }
            
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            TakeDamage(1, -facingDir);
        }
        
        HandleCollision();
        HandleAnimator();

        idleTimer -= Time.deltaTime;
    }

    void UpdatePlayerRef()
    {
        if(player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    protected override void Move()
    {
        
    }

    public override void Attack() 
    {
        // anim.SetTrigger("attack");
    }

    public override void TakeDamage(int damage, int hitDir)
    {
        if (isKnockBack) return;

        currentHP -= damage;

        if(currentHP == 0)
        {
            Die();
        }
        else
        {
            // anim.SetTrigger("hit");
            Knockback(hitDir);
        }
    }

    void Knockback(int hitDir)
    {
        sr.color = Color.red;
        StartCoroutine(KnockbackRoutine());

        rb.linearVelocity = new Vector2(knockbackForce.x * hitDir, knockbackForce.y);
    }

    protected override void Die()
    {
        isDead = true;
        col.enabled = false;
        if (damageTrigger != null) damageTrigger.SetActive(false);
        // anim.SetTrigger("die");

        rb.linearVelocity = new Vector2(0, deathImpact);
        if (Random.Range(0, 100) < 50) deathRotationDir *= -1;

        // 애니메이션에서 실행
        // gameObject.SetActive(false);

        // 돈 드랍
    }

    void HandleDeathRotation()
    {
        transform.Rotate(0, 0, deathRotationSpeed * deathRotationDir * Time.deltaTime);
    }

    /*
    protected virtual void HandleTurn(float x)
    {
        if (x < 0 && !facingLeft || x > 0 && facingLeft)
            Turn();
    }*/

    protected virtual void Turn()
    {
        facingDir *= -1;
        transform.Rotate(0, 180, 0);
        facingLeft = !facingLeft;
    }

    protected virtual void HandleAnimator()
    {
        // anim.SetFloat("velocityX", rb.linearVelocityX);
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

    IEnumerator KnockbackRoutine()
    {
        isKnockBack = true;

        yield return new WaitForSeconds(knockbackDuration);
        isKnockBack = false;
        GetComponent<SpriteRenderer>().color = defaultColor;
    }
}
