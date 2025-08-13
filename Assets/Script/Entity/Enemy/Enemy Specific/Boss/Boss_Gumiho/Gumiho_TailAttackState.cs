using UnityEngine;

public class Gumiho_TailAttackState : MeleeAttackState
{
    Boss_Gumiho gumiho;

    PlayerManager playerManager;

    [SerializeField] float attackCooldown;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "tailAttack";
        gumiho = enemy as Boss_Gumiho;   
    }

    public override void Enter()
    {
        base.Enter();

        // TODO: playerManager = Singleton.GameManager_Instance.Get<PlayerManager>();
        playerManager = gumiho.player.GetComponent<PlayerManager>();

        gumiho.CanBeKnockedBack = false;
        gumiho.MoveState.isClawAttackCancelled = false;

        enemy.SetVelocity(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        gumiho.IdleState.SetIdleTime(attackCooldown);

        if(isAnimationFinished)
        {
            stateMachine.ChangeState(gumiho.IdleState);
        }
    }

    public override void TriggerAttack()
    {
        Collider2D[] detectedObjs = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius, playerLayer);

        foreach (var col in detectedObjs)
        {
            // 플레이어 공격
            //col.GetComponent<PlayerManager>()?.TakeDamage(1, enemy.aliveGO.transform.position);
            Debug.Log($"{enemy.gameObject.name}가 플레이어를 밀침");
            playerManager.Knockback(enemy.aliveGO.transform.position, 4, 1);
        }
    }

    public override void Exit()
    {
        base.Exit();

        gumiho.CanBeKnockedBack = true;
    }
}