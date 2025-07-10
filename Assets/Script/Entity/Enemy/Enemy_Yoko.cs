using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Enemy_Yoko : Enemy
{
    protected override void Update() 
    {
        base.Update();

        if(isDead) return;

        Move();
        if(isGrounded) Turn();
    }

    protected override void Move()
    {
        if (idleTimer > 0) return;
        
        rb.linearVelocity = new Vector2(walkSpeed * facingDir, rb.linearVelocityY);
    }

    protected override void Turn()
    {
        if(!isFrontGrounded || isWall)
        {
            base.Turn();
            idleTimer = idleDuration;
            rb.linearVelocity = Vector2.zero;
        }   
    }

    void Jump()
    {

    }
}
