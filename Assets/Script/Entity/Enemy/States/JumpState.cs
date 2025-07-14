using UnityEditor.Callbacks;
using UnityEngine;

public class JumpState : State
{
    protected D_JumpState stateData;

    // protected Transform player;

    protected bool isGrounded;
    protected bool isJumpDone;

     protected bool isPlayerMinRange;
    protected bool isLedge;
    protected bool isWall;
    protected bool performCloseRangeAction;

    public JumpState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_JumpState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isGrounded = enemy.CheckGround();

        isPlayerMinRange = enemy.CheckPlayerMinRange();
        isLedge = enemy.ChackLedge();
        isWall = enemy.ChackWall();

        performCloseRangeAction = enemy.CheckPlayerInCloseRangeAction();
    }
    public override void Enter()
    {
        base.Enter();

        isJumpDone = false;
        isGrounded = false;

        // Jump
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isGrounded && !isJumpDone && Time.time >= startTime + 0.2f)
        {
            isJumpDone = true;
            enemy.SetVelocity(0f);
            OnJumpLanding();
        }
    }

    protected virtual void OnJumpLanding()
    {
        // 착지 후 상태 결정
    }

}
