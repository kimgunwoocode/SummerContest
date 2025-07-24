using UnityEngine;

public class DeadState : State
{
    [SerializeField, Tooltip("파편이 생성되는 파티클.")] 
    protected GameObject deathChunkParticle;

    [SerializeField, Tooltip("혈흔이 생성되는 파티클.")] 
    protected GameObject deathBloodParticle;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "dead";
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();

        if(deathBloodParticle != null)
            GameObject.Instantiate(deathBloodParticle, enemy.aliveGO.transform.position, deathBloodParticle.transform.rotation);
        
        if(deathChunkParticle != null)
            GameObject.Instantiate(deathChunkParticle, enemy.aliveGO.transform.position, deathChunkParticle.transform.rotation);

        enemy.gameObject.SetActive(false);
        // TODO: 추후 리스폰, 돈 드랍 등을 반영하여 수정 필요
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
