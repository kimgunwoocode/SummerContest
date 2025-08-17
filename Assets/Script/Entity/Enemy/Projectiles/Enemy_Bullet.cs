using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sp;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    public void SetVelocity(Vector2 velocity)
    {
        rb.linearVelocity = velocity;
    }

    public void SetFlipX()
    {
        sp.flipX = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌 시 공격 처리
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            if (!playerManager.IsInvincible)
            {
                Debug.Log("도깨비불에 공격 당함");
                playerManager.TakeDamage(1, transform.position);
                //gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}