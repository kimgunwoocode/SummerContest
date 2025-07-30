using System.Collections.Generic;
using UnityEngine;

public class Gumiho_MoveState : Boss_MoveState
{
    Boss_Gumiho gumiho;

    private State lastAttack;
    public bool isClawAttackCancelled = false;

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

        gumiho.LookAtPlayer();

        // 공격 로직
        if (enemy.currentHP > gumiho.phase2HP)  // 1 Phase
        {
            ChoosePhase1Attack(); // 근접 공격 Phase 1 패턴
        }
        else // 2 Phase
        {
            ChoosePhase2Attack(); // 근접, 원거리 공격 Phase 1 패턴
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!isGround && gumiho.player == null) return;

        Vector2 target = new Vector2(gumiho.player.position.x, enemy.rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(enemy.rb.position, target, moveSpeed * Time.fixedDeltaTime);
        enemy.rb.MovePosition(newPos);
    }

    /// <summary>
    /// 페이즈 1 공격 로직
    /// </summary>
    void ChoosePhase1Attack()
    {
        if (performCloseRangeAction) // 근접 공격 범위
        {
            ChooseRandomMeleeAttack();
        }
    }

    /// <summary>
    /// 페이즈 2 공격 로직
    /// </summary>
    void ChoosePhase2Attack()
    {
        // 발톱 할퀴기가 경직으로 캔슬됐을 경우 꼬리치기 발동
        if (isClawAttackCancelled)
        {
            isClawAttackCancelled = false;
            lastAttack = gumiho.TailAttackState;
            stateMachine.ChangeState(gumiho.TailAttackState); // 꼬리치기 실행
            Debug.Log("발톱공격 캔슬로 꼬리치기 반격");
        }
        else if (lastAttack == gumiho.TailAttackState) // 꼬리치기 이후에는 원거리 공격 실행
        {
            ChooseRandomRangedAttack();
        }
        else if (performCloseRangeAction) // 근접 공격 범위
        {
            ChooseRandomMeleeAttack();
        }
        else if (isPlayerMaxRange && !isPlayerMinRange) // 원거리 공격 범위
        {
            ChooseRandomRangedAttack();
        }
    }

    /// <summary>
    /// 근접 공격인 발톱 할퀴기, 꼬리치기를 3:1 비율로 선택함
    /// </summary>
    void ChooseRandomMeleeAttack()
    {
        // 0부터 3까지의 정수 중 하나를 무작위로 선택
        int randomNumber = Random.Range(0, 4);

        if (randomNumber >= 0 && randomNumber <= 2) // 0, 1, 2 (3가지 경우)
        {
            lastAttack = gumiho.ClawAttackState;
            stateMachine.ChangeState(gumiho.ClawAttackState); // 발톱 할퀴기 실행
        }
        else // 3 (1가지 경우)
        {
            lastAttack = gumiho.TailAttackState;
            stateMachine.ChangeState(gumiho.TailAttackState); // 꼬리치기 실행
        }
    }
    
    /// <summary>
    /// 원거리 공격 3가지, 돌진을 3:3:3:1 비율로 선택함
    /// </summary>
    void ChooseRandomRangedAttack()
    {
        // 0~9 범위에서 랜덤한 값을 선택하여 각 공격에 대한 확률을 설정
        int randomNumber = Random.Range(0, 10); // 0 ~ 9

        // 중복 방지를 위해 공격이 이미 선택되었는지 확인하는 변수
        State chosenAttack = null;

        if (randomNumber < 3) // 0~2 => FoxOrbAttackState
        {
            if (lastAttack != gumiho.FoxOrbAttackState)
            {
                chosenAttack = gumiho.FoxOrbAttackState;
            }
        }
        else if (randomNumber < 6) // 3~5 => FoxFireAttackState
        {
            if (lastAttack != gumiho.FoxFireAttackState)
            {
                chosenAttack = gumiho.FoxFireAttackState;
            }
        }
        else if (randomNumber < 9) // 6~8 => SpiritLeapAttackState
        {
            if (lastAttack != gumiho.SpiritLeapAttackState)
            {
                chosenAttack = gumiho.SpiritLeapAttackState;
            }
        }
        else // 9 => ChargeState
        {
            chosenAttack = gumiho.ChargeState;
        }

        // 만약 선택된 공격이 없으면 다시 선택하기
        if (chosenAttack == null)
        {
            ChooseRandomRangedAttack(); // 다시 선택
            return;
        }

        // 선택한 공격으로 상태 변경
        lastAttack = chosenAttack;
        stateMachine.ChangeState(chosenAttack);
    }
}