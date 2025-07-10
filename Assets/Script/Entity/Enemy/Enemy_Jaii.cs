using UnityEngine;

public class Enemy_Jaii : Enemy
{
    [Header("Jaii Details")]
    [SerializeField] float speedUpRate = .6f;
    [SerializeField] float detectionRange;

    float defaultSpeed;
    bool canMove;

    protected override void Start()
    {
        base.Start();

        defaultSpeed = walkSpeed;
    }

    protected override void Update()
    {
        base.Update();

        if (isDead || isKnockBack) return;

        HandleCharge();
    }

    void HandleCharge()
    {
        if(!canMove || idleTimer > 0) return;

        walkSpeed += speedUpRate * Time.deltaTime;
        if(walkSpeed >= runSpeed) walkSpeed = runSpeed;

        rb.linearVelocity = new Vector2(walkSpeed * facingDir, rb.linearVelocityY);

        if(isWall) HandleWallHit();

        if(!isFrontGrounded) TurnAround();
    }

    void HandleWallHit()
    {
        canMove = false;
        idleTimer = idleDuration;
        // anim.SetBool("hitWall", true);
        ResetSpeed();
        TakeDamage(0, -facingDir);
        Turn();
    }

    void TurnAround()
    {
        ResetSpeed();

        idleTimer = idleDuration;
        rb.linearVelocity = Vector2.zero;
        Turn();
    }
    
    void ResetSpeed()
    {
        walkSpeed = defaultSpeed;
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();

        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, detectionRange, playerLayer);

        if (isPlayerDetected && isGrounded) canMove = true;
    }

    protected override void OnDrawGizmos() 
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (detectionRange * facingDir), transform.position.y));
    }
}