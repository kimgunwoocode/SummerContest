using UnityEngine;

public class NomalBreath : BreathObject
{
    public float speed;
    [Header("부딪혀 사라지게 할 레이어")]
    [SerializeField] private LayerMask hitLayers;
    //디버깅용(+ layer 사용 제안)
    [SerializeField] private LayerMask enemyLayer;

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
        //디버깅을 위한 잠깐의 코드 수정.
        if (collision.gameObject.layer == enemyLayer)//CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyEntity>()?.TakeDamage(Singleton.GameManager_Instance.Get<GameDataManager>().ATK, transform.position);
            Destroy(gameObject);
        }
        else if (((1 << collision.gameObject.layer) & hitLayers) != 0)
        {
            Destroy(gameObject);
        }
    }

    //enemy가 collider를 갖고있지 않아 Trigger이벤트에 잡히지 않음.
    //이에따라 일시적으로 만든 코드이므로 삭제 권고
    private void EnemyCheck() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, gameObject.GetComponent<CircleCollider2D>().radius, enemyLayer);
        foreach (var hit in hits) {
            Debug.Log(hit.name);
            hit.GetComponentInParent<EnemyEntity>()?.TakeDamage(Singleton.GameManager_Instance.Get<GameDataManager>().ATK, transform.position);
        }
    }

    private void Update() {
        EnemyCheck();
    }
}
