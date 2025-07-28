using UnityEngine;

public class NomalBreath : BreathObject
{
    public float speed;
    [Header("부딪혀 사라지게 할 레이어")]
    [SerializeField] private LayerMask hitLayers;

    [Space]
    public Rigidbody2D rb;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + shootingDirection * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyEntity>()?.TakeDamage(Singleton.GameManager_Instance.Get<GameDataManager>().ATK, transform.position);
            Destroy(gameObject);
        }
        else if (((1 << collision.gameObject.layer) & hitLayers) != 0)
        {
            Destroy(gameObject);
        }
    }
}
