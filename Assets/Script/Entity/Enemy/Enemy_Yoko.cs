using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Enemy_Yoko : Enemy
{
    [Header("Yoko Details")]
    [SerializeField] float detectionRange;
    [SerializeField] float crouchDuration = 0.5f;
    [SerializeField] Vector2 jumpForce = new Vector2(4, 6);

    bool isCrouching;
    Vector2 currentPlayer;

    protected override void Update()
    {
        base.Update();

        if (isDead || isKnockBack) return;

        if (isPlayerDetected && !isCrouching && idleTimer <= 0)
        {
            PrepareAttack();
            return;
        }


        if (!isCrouching) Move();

        if (isGrounded && (!isFrontGrounded || isWall)) Turn();
    }

    void PrepareAttack()
    {
        isCrouching = true;
        idleTimer = crouchDuration;
        rb.linearVelocity = Vector2.zero;

        // anim.SetTrigger("ready");
        Invoke(nameof(Attack), crouchDuration);
    }

    public override void Attack()
    {
        base.Attack();
        Jump();
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(jumpForce.x * facingDir, jumpForce.y);
        isCrouching = false;
    }

    protected override void Move()
    {
        if (idleTimer > 0 || isKnockBack) return;

        rb.linearVelocity = new Vector2(walkSpeed * facingDir, rb.linearVelocityY);
    }

    protected override void Turn()
    {
        base.Turn();
        idleTimer = idleDuration;
        rb.linearVelocity = Vector2.zero;
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();

        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, detectionRange, playerLayer);
    }
    
    protected override void OnDrawGizmos() 
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (detectionRange * facingDir), transform.position.y));
    }
}
