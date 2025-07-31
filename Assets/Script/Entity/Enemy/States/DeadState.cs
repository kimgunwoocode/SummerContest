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

        if (deathBloodParticle != null)
            GameObject.Instantiate(deathBloodParticle, enemy.aliveGO.transform.position, Quaternion.identity, enemy.transform);

        if (deathChunkParticle != null)
            GameObject.Instantiate(deathChunkParticle, enemy.aliveGO.transform.position, Quaternion.identity, enemy.transform);

        // 리워드 수령
        Singleton.GameManager_Instance.Get<GameManager>().Get_Money(enemy.enemyData.minCoinReward, enemy.enemyData.maxCoinReward + 1);

        Debug.Log($"{enemy.enemyData.enemyName}이 죽음");
        Invoke(nameof(DisableEnemy), 2f);
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
    
    void DisableEnemy()
    {
        enemy.gameObject.SetActive(false);
    }
}
