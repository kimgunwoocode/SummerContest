using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gumiho_SpiritLeapAttackState : AttackState
{
    [SerializeField] float attackCooldown;
    
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float airAttackDelay = 0.5f; // 점프 후 여우불 발사까지 시간

    [Header("Fox Orb Details")]
    [SerializeField] private GameObject foxOrbPrefab;
    [SerializeField] private float foxOrbSpeed = 2f;
    [SerializeField] private float foxOrbLifeTime = 3f;
    [SerializeField] private int numberOfOrbs = 5;

    private Boss_Gumiho gumiho;
    private List<GameObject> foxFires;  // 여우불 오브젝트를 저장할 리스트
    private bool hasAttacked = false;
    private bool hasJumped = false;

    private bool isGrounded;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "spiritLeapAttack";
        gumiho = enemy as Boss_Gumiho;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isGrounded = enemy.CheckGround();
    }

    public override void Enter()
    {
        base.Enter();

        gumiho.canBeKnockedBack = false;

        hasAttacked = false;
        hasJumped = false;

        Jump();
    }

    private void Jump()
    {
        if (!hasJumped)
        {
            hasJumped = true;
            enemy.rb.linearVelocity = new Vector2(enemy.rb.linearVelocity.x, jumpForce);
            StartCoroutine(WaitAndAttack());
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // 공격 끝났으면 상태 전환
        if (isAnimationFinished && isGrounded)
        {
            gumiho.IdleState.SetIdleTime(attackCooldown);
            stateMachine.ChangeState(gumiho.IdleState);
        }
    }

    public override void TriggerAttack()
    {
        if (hasAttacked) return;
        hasAttacked = true;
        gumiho.canBeKnockedBack = true;

        base.TriggerAttack();

        foxFires = new List<GameObject>();

        // 플레이어 방향 계산
        Vector2 baseDirection = (gumiho.player.position - attackPosition.position).normalized;

        // 방사형 각도 설정
        float angleBetweenOrbs = 20f; // 중앙 기준으로 20도 간격

        // 여우불 5개 생성
        for (int i = 0; i < numberOfOrbs; i++)
        {
            GameObject foxFire = Instantiate(foxOrbPrefab, attackPosition.position, Quaternion.identity);
            foxFires.Add(foxFire);

            // 중앙 여우불은 그대로 발사
            if (i == numberOfOrbs / 2)
            {
                foxFire.GetComponent<Rigidbody2D>().linearVelocity = baseDirection * foxOrbSpeed;
            }
            else
            {
                // 인덱스를 기준으로 좌우로 퍼지도록 각도 계산
                int offsetFromCenter = i - numberOfOrbs / 2;
                float angle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;
                float spreadAngle = angle + angleBetweenOrbs * offsetFromCenter;
                Vector2 dir = new Vector2(Mathf.Cos(spreadAngle * Mathf.Deg2Rad), Mathf.Sin(spreadAngle * Mathf.Deg2Rad)).normalized;

                foxFire.GetComponent<Rigidbody2D>().linearVelocity = dir * foxOrbSpeed;
            }
        }

        // 여우불 life time 후 삭제
        StartCoroutine(MoveFoxFires());
    }

    public override void FinishAttack()
    {
        for (int i = 0; i < foxFires.Count; i++)
        {
            if (foxFires[i] != null) Destroy(foxFires[i]);
        }

        base.FinishAttack();
    }

    private IEnumerator WaitAndAttack()
    {
        yield return new WaitForSeconds(airAttackDelay);
        TriggerAttack();
    }

    private IEnumerator MoveFoxFires()
    {
        yield return new WaitForSeconds(foxOrbLifeTime);
        FinishAttack();
    }
}