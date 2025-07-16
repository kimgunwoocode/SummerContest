using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    [SerializeField] float attackRadius = .5f;
    
    [SerializeField] protected int attackDamage = 1;
    [SerializeField] protected LayerMask playerLayer;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "meleeAttack";
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();
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

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        Collider2D[] detectedObjs = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius, playerLayer);

        foreach(Collider2D col in detectedObjs)
        {
            // 플레이어 공격
            // col.GetComponent<PlayerManager>()?.TakeDamage(1, enemy.aliveGO.transform.position);
            Debug.Log("적이 플레이어를 공격함");
        }
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
}
