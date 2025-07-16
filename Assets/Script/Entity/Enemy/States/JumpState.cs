using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class JumpState : State
{
    [SerializeField] protected float jumpHeight = 3f;
    [SerializeField] protected float jumpOffset = .5f;

    protected bool isGrounded;
    protected bool isJumpDone;

     protected bool isPlayerMinRange;
    protected bool isLedge;
    protected bool isWall;
    protected bool performCloseRangeAction;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "jump";
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isGrounded = enemy.CheckGround();

        isPlayerMinRange = enemy.CheckPlayerMinRange();
        isLedge = enemy.ChackLedge();
        isWall = enemy.CheckWall();

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
