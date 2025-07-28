using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(Vector2 velocity)
    {
        rb.linearVelocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // 플레이어와 충돌 시 공격 처리
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("도깨비불 공격");
            // 플레이어에게 데미지 처리 (예시 코드)
            // other.GetComponent<PlayerManager>().TakeDamage(1, transform.position);
            Destroy(gameObject);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }    
    }
}