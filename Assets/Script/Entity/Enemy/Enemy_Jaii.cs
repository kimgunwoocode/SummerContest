using UnityEngine;

public class Enemy_Jaii : Enemy
{
    [Header("Jaii Details")]
    [SerializeField] float speedUpRate = .6f;
    [SerializeField] float detectionRange;

    float defaultSpeed;
    Color defaultColor;

    protected override void Start()
    {
        base.Start();

        defaultSpeed = walkSpeed;
        defaultColor = GetComponent<SpriteRenderer>().color;
    }

    protected override void Update()
    {
        base.Update();

        HandleCharge();
    }

    void HandleCharge()
    {
        if(!isKnockBack) return;

        walkSpeed += speedUpRate * Time.deltaTime;
        if(walkSpeed >= runSpeed) walkSpeed = runSpeed;

        rb.linearVelocity = new Vector2(walkSpeed * facingDir, rb.linearVelocityY);

        if(isWall) WallHit();

        if(!isFrontGrounded)
        {
            TurnAround();
        }
    }

    void TurnAround()
    {
        walkSpeed = defaultSpeed;
        isKnockBack = false;
        rb.linearVelocity = Vector2.zero;
        Stun();
    }

    void WallHit()
    {
        isKnockBack = false;
        walkSpeed = defaultSpeed;
        // anim.SetBool("hitWall", true);
        rb.linearVelocity = new Vector2(knockbackForce.x * -facingDir, knockbackForce.y);
        Stun();
    }

    // 기절 확인용 임시 코드
    void Stun()
    {
        // anim.SetBool("hitWall", false);
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke(nameof(Turn), knockbackDuration);
        Invoke(nameof(ResetColor), knockbackDuration);
    }

    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = defaultColor;
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();

        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, detectionRange, playerLayer);

        if(isPlayerDetected && isGrounded) isKnockBack = true;
    }

    protected override void OnDrawGizmos() 
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (detectionRange * facingDir), transform.position.y));
    }
}
