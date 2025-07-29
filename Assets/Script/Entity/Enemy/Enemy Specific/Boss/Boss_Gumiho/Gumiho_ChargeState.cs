using UnityEngine;

public class Gumiho_ChargeState : ChargeState
{
    Boss_Gumiho gumiho;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "move"; // TODO: 달리기로 수정
        gumiho = enemy as Boss_Gumiho;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction || isChargeTimeOver || !isLedge || isWall)
        {
            stateMachine.ChangeState(gumiho.MoveState);
        }
    }
}
