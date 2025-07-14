using UnityEngine;

public class DeadState : State
{
   protected D_DeadState stateData;

    public DeadState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();

        if(stateData.deathBloodParticle != null)
            GameObject.Instantiate(stateData.deathBloodParticle, enemy.aliveGO.transform.position, stateData.deathBloodParticle.transform.rotation);
        
        if(stateData.deathChunkParticle != null)
            GameObject.Instantiate(stateData.deathChunkParticle, enemy.aliveGO.transform.position, stateData.deathChunkParticle.transform.rotation);

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
