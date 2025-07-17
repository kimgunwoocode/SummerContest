using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    Rigidbody2D rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    public void SetVelocity(Vector2 velocity) => rb.linearVelocity = velocity;

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // player 공격
            // other.GetComponent<PlayerManager>().TakeDamage(1, transform.position);
            Destroy(gameObject);
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }    
    }
}
