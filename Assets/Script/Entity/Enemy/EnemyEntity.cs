using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyEntity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;
    public D_Enemy enemyData;
    
    public Rigidbody2D rb {get; private set;}
    public Animator anim {get; private set;}
    public GameObject aliveGO {get; private set;}
    public int facingDir {get; private set;} = -1;
    public AnimationToStatemachine atsm {get; private set;}
    public int lastDamageDirection {get; private set;}

    [SerializeField] SpriteRenderer spriteRenderer;
    
    [Header("Check")]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected Transform ledgeCheck;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform playerCheck;

    protected bool isDead;
    protected float lastDamageTime;    

    Vector2 velocityWorkspace;
    int currentHP;

    [ContextMenu("Change Facing Direction")]
    public void FlipDefaultFacingDir()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    protected virtual void Awake()
    {
        currentHP = enemyData.maxHP;

        aliveGO = transform.Find("Alive").gameObject;
        rb = aliveGO.GetComponent<Rigidbody2D>();
        anim = aliveGO.GetComponent<Animator>();
        atsm = aliveGO.GetComponent<AnimationToStatemachine>();

        stateMachine = new FiniteStateMachine();

        foreach (var state in GetComponents<State>())
        {
            state.Initialize(this, stateMachine);
        }

        if (spriteRenderer.flipX && facingDir == -1)
        {
            spriteRenderer.flipX = false;
            Flip();
        }
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();

        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            TakeDamage(1, Vector2.zero);
        }
    }

    protected virtual void FixedUpdate() 
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(velocity * facingDir, rb.linearVelocityY);
        rb.linearVelocity = velocityWorkspace;
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.linearVelocity = velocityWorkspace;
    }

    public virtual void SetVelocity(float velocityX, float velocityY)
    {
        velocityWorkspace.Set(velocityX * facingDir, velocityY);
        rb.linearVelocity = velocityWorkspace;
    }

    public virtual void TakeDamage(int damageAmount, Vector2 attackerPosition)
    {
        lastDamageTime = Time.time;

        currentHP -= damageAmount;

        if(enemyData.hitParticle != null)
        {
            Instantiate(enemyData.hitParticle, aliveGO.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        }

        if(attackerPosition.x > aliveGO.transform.position.x)
        {
            lastDamageDirection = -1;
        }
        else
        {
            lastDamageDirection = 1;
        }

        if(currentHP <= 0)
        {
            isDead = true;
        }
    }
    public virtual void Flip()
    {
        facingDir *= -1;
        aliveGO.transform.Rotate(0, 180, 0);
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, -aliveGO.transform.right, enemyData.wallCheckDistance, enemyData.groundLayer);
    }

    public virtual bool ChackLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, enemyData.ledgeCheckDistance, enemyData.groundLayer);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, enemyData.groundCkeckRadius, enemyData.groundLayer);
    }

    public virtual bool CheckPlayerMinRange()
    {
        return Physics2D.Raycast(playerCheck.position, -aliveGO.transform.right, enemyData.minPlayerCheckDistance, enemyData.playerLayer);
    }

    public virtual bool CheckPlayerMaxRange()
    {
        return Physics2D.Raycast(playerCheck.position, -aliveGO.transform.right, enemyData.maxPlayerCheckDistance, enemyData.playerLayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, -aliveGO.transform.right, enemyData.closeRangeActionDistance, enemyData.playerLayer);
    }

    public virtual bool CheckPlayerBehind()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, enemyData.minPlayerCheckDistance, enemyData.playerLayer);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * enemyData.ledgeCheckDistance));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDir * enemyData.wallCheckDistance));
        
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDir * enemyData.closeRangeActionDistance), .2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDir * enemyData.minPlayerCheckDistance), .2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDir * enemyData.maxPlayerCheckDistance), .2f);

        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(-Vector2.right * facingDir * enemyData.minPlayerCheckDistance), .2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, enemyData.groundCkeckRadius);
    }
}