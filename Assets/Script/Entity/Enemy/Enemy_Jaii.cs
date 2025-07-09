using UnityEngine;

public class Enemy_Jaii : Enemy
{
    [Header("Jaii Details")]
    [SerializeField] float speedUpRate = .6f;
    [SerializeField] float detectionRange;
    [SerializeField] Vector2 impactPower;

    float defaultSpeed;
    Color defaultColor;
    bool playerDetected;

    void Start() 
    {
        defaultSpeed = walkSpeed;
        defaultColor = GetComponent<SpriteRenderer>().color;
    }

    protected override void Update()
    {
        base.Update();

        // anim.SetFloat("velocityX", rb.linearVelocityX);

        HandleCollision();
        HandleCharge();
    }

    void HandleCharge()
    {
        if(!canMove) return;

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
        canMove = false;
        rb.linearVelocity = Vector2.zero;
        Stun();
    }

    void WallHit()
    {
        canMove = false;
        walkSpeed = defaultSpeed;
        // anim.SetBool("hitWall", true);
        rb.linearVelocity = new Vector2(impactPower.x * -facingDir, impactPower.y);
        Stun();
    }

    // 기절 확인용 임시 코드
    void Stun()
    {
        // anim.SetBool("hitWall", false);
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke(nameof(Turn), idleDuration);
        Invoke(nameof(ResetColor), idleDuration);
    }

    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = defaultColor;
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();

        playerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, detectionRange, playerLayer);

        if(playerDetected && isGrounded) canMove = true;
    }


    protected override void OnDrawGizmos() 
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (detectionRange * facingDir), transform.position.y));
    }

}
