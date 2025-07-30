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
    private float elapsedTime = 0f;

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
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // 구슬의 상태 업데이트 (발사 후, 돌아오는 상태)
        if (!isStopped && !isReturning)
        {
            ApplyThrowMovement();
        }
        else if (isStopped && !isReturning)
        {
            HandleReturnDelay();
        }
        else if (isReturning)
        {
            ApplyReturnMovement();
        }
    }

    // 발사된 구슬의 속도 변화 처리 (easing 적용)
    private void ApplyThrowMovement()
    {
        float x = elapsedTime / throwTime;
        rb.linearVelocity = easeInSine(x) * initialVelocity.magnitude * initialVelocity.normalized; // 발사되는 구슬의 속도 변화

        // 수명이 다하면 구슬을 멈추고 돌아오는 딜레이 시작
        if (elapsedTime >= throwTime)
        {
            elapsedTime = 0f;
            StopAndDelayReturn();
        }
    }

    // 구슬이 멈추고 돌아오는 딜레이 처리
    private void HandleReturnDelay()
    {
        if (elapsedTime >= returnDelay)
        {
            isReturning = true;
            elapsedTime = 0f;  // 돌아올 때 경과 시간 초기화
        }
    }

    // 돌아오는 구슬의 속도 변화 처리 (easing 적용)
    private void ApplyReturnMovement()
    {
        float x = elapsedTime / throwTime;
        rb.linearVelocity = easeOutSine(x) * initialVelocity.magnitude * -initialVelocity.normalized; // 돌아오는 구슬의 속도 변화
    }

    // 구슬을 멈추고 돌아오는 딜레이를 시작
    private void StopAndDelayReturn()
    {
        rb.linearVelocity = Vector2.zero; // 구슬 속도 멈춤
        isStopped = true;
    }

    static float easeInSine(float value)
    {
        return Mathf.Cos(value * (Mathf.PI * 0.5f));
    }

    static float easeOutSine(float value)
    {
        return Mathf.Sin(value * (Mathf.PI * 0.5f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌 시 공격 처리
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            if (!playerManager.IsInvincible)
            {
                Debug.Log("여우구슬 공격");
                playerManager.TakeDamage(1, transform.position);
            }
        }
    }
}