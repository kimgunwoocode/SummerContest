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
        SpawnPhase2Fx();
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

    private void SpawnPhase2Fx()
    {
        if (!phase2Particle)
        {
            Debug.Log("Gumiho phase2Particle is null");
            return;
        }

        // 보스 위치에 FX 생성
        ParticleSystem fx = Instantiate(
            phase2Particle, 
            enemy.aliveGO.transform.position, 
            Quaternion.identity);

        // 원샷 파티클 자동 삭제 세팅
        var main = fx.main;
        main.loop = false;
        main.stopAction = ParticleSystemStopAction.Destroy;

        fx.Play();
    }

    private void PlayerPush()
    {
        // TODO: Singleton.GameManager_Instance.Get<PlayerManager>();
        gumiho.player.GetComponent<PlayerManager>().Knockback(enemy.aliveGO.transform.position, 4, 1);
    }
}
