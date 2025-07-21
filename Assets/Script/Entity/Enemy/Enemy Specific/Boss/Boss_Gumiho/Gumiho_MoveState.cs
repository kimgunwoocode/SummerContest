using UnityEngine;

public class Gumiho_MoveState : Boss_MoveState
{
    Boss_Gumiho gumiho;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        gumiho = enemy as Boss_Gumiho;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(performCloseRangeAction)
        {
            if(enemy.currentHP > gumiho.phase2HP) // 1 Phase
            {
                ChoosePhase1Attack();
            }
            else // 2 Phase
            {

            }
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        gumiho.LookAtPlayer();

        Vector2 target = new Vector2(gumiho.player.position.x, enemy.rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(enemy.rb.position, target, moveSpeed * Time.fixedDeltaTime);
        enemy.rb.MovePosition(newPos);
    }

    /// <summary>
    /// 페이즈 1 공격 로직
    /// 근접 공격인 발톱 할퀴기, 꼬리치기를 3:1 비율로 선택함
    /// </summary>
    void ChoosePhase1Attack()
    {
        // 0부터 3까지의 정수 중 하나를 무작위로 선택
        int randomNumber = Random.Range(0, 4); // 0, 1, 2, 3

        if (randomNumber >= 0 && randomNumber <= 2) // 0, 1, 2 (3가지 경우)
        {
            stateMachine.ChangeState(gumiho.ClawAttackState);
        }
        else // 3 (1가지 경우)
        {
            stateMachine.ChangeState(gumiho.TailAttackState);
        }
    }

    /// <summary>
    /// 페이즈 2 공격 로직
    /// 
    /// </summary>
    void ChoosePhase2Attack()
    {
        // TODO: 원거리 공격 구현 후 공격 로직 짜기
    }
}
