using UnityEngine;

public class Gumiho_FoxOrb : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 initialVelocity;

    private float fireTime;
    private float returnStartTime;
    private float throwTime;
    private float returnDelay;

    private bool isStopped = false;
    private bool isReturning = false;

    /// <summary>
    /// 여우구슬 초기화 함수
    /// </summary>
    /// <param name="velocity">구슬 발사 초기 속도</param>
    /// <param name="lifeTime">구슬이 던져지는 시간</param>
    /// <param name="delayTime">돌아오는 딜레이 시간</param>
    public void Initialize(Vector2 velocity, float lifeTime, float delayTime)
    {
        initialVelocity = velocity;
        throwTime = lifeTime;
        returnDelay = delayTime;
        fireTime = Time.time;
        
        rb.linearVelocity = velocity;  // 초기 속도 설정
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 수명이 끝났다면 구슬의 속도를 0으로 하고, 돌아오기 딜레이를 시작
        if (Time.time >= fireTime + throwTime && !isStopped)
        {
            rb.linearVelocity = Vector2.zero; // 구슬 속도 멈춤
            returnStartTime = Time.time; // 돌아오기 딜레이 시작
            isStopped = true;
        }
        // 돌아오는 딜레이가 지나면 구슬이 다시 돌아옴
        else if (Time.time >= returnStartTime + returnDelay && !isReturning && returnStartTime > 0.1f)
        {
            rb.linearVelocity = -initialVelocity; // 구슬이 반대로 돌아옴
            isReturning = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌 시 공격 처리
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("여우구슬 공격");
            // 플레이어에게 데미지 처리 (예시 코드)
            // other.GetComponent<PlayerManager>().TakeDamage(1, transform.position);
        }
    }
}