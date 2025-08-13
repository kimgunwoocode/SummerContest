using UnityEngine;

public class Gumiho_Phase2State : Boss_PhaseChangeState
{
    Boss_Gumiho gumiho;

    [SerializeField] private ParticleSystem phase2Particle;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "phase2";
        gumiho = enemy as Boss_Gumiho;
    }

    public override void Enter()
    {
        base.Enter();

        gumiho.CanBeKnockedBack = false;
        gumiho.MoveState.isClawAttackCancelled = false;

        // 페이즈 전환 효과
        phase2Particle.Play();
        PlayerPush();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        gumiho.LookAtPlayer();

        if (isPhaseChangeTimeOver)
        {
            stateMachine.ChangeState(gumiho.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        gumiho.CanBeKnockedBack = true;
    }

    private void PlayerPush()
    {
        // TODO: Singleton.GameManager_Instance.Get<PlayerManager>();
        gumiho.player.GetComponent<PlayerManager>().Knockback(enemy.aliveGO.transform.position, 4, 1);
    }
}
