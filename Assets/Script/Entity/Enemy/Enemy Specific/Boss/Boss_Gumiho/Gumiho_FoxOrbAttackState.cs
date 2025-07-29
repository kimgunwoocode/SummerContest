using UnityEngine;

public class Gumiho_FoxOrbAttackState : AttackState
{
    [SerializeField] float attackCooldown;

    [SerializeField] private Gumiho_FoxOrb foxOrbPrefab;
    [SerializeField] private float foxOrbSpeed = 5f;

    [SerializeField, Tooltip("여우구슬이 돌아오는 딜레이 시간")]
    private float foxOrbReturnDelay = 0.5f;

    [SerializeField, Tooltip("여우구슬이 던져지는 시간")]
    private float foxOrbThrowTime = 1f;

    private Boss_Gumiho gumiho;
    private float totalFoxOrbLifetime;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "foxOrbAttack";
        gumiho = enemy as Boss_Gumiho;
    }

    public override void Enter()
    {
        base.Enter();

        gumiho.canBeKnockedBack = false;
        // 구슬이 던져지고 돌아오기까지의 전체 시간 계산
        totalFoxOrbLifetime = foxOrbThrowTime * 2 + foxOrbReturnDelay;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + totalFoxOrbLifetime && !isAnimationFinished)
        {
            FinishAttack();
        }

        if (isAnimationFinished)
        {
            gumiho.IdleState.SetIdleTime(attackCooldown);
            stateMachine.ChangeState(gumiho.IdleState);
        }
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        gumiho.canBeKnockedBack = true;
        
        FoxOrbAttack();
    }

    public void FoxOrbAttack()
    {
        // FoxOrb 인스턴스 생성
        Gumiho_FoxOrb newOrb = Instantiate(foxOrbPrefab, attackPosition.position, Quaternion.identity);

        // 구슬 초기 속도 설정, 초기화
        Vector2 foxOrbVelocity = new Vector2(foxOrbSpeed, 0);
        newOrb.Initialize(foxOrbVelocity * enemy.facingDir, foxOrbThrowTime, foxOrbReturnDelay);

        // 구슬의 Total Lifetime 후 삭제되도록 설정
        Destroy(newOrb.gameObject, totalFoxOrbLifetime);
    }
}