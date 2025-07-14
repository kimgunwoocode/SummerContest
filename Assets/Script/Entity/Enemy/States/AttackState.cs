using UnityEngine;

public class AttackState : State
{
    protected Transform attackPosition;

    protected bool isAnimationFinished;
    protected bool isPlayerMinRange;
    // protected bool isPlayerMaxRange;

    public AttackState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition) : base(enemy, stateMachine, animBoolName)
    {
        this.attackPosition = attackPosition;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isPlayerMinRange = enemy.CheckPlayerMinRange();
        // isPlayerMinRange = enemy.CheckPlayerMaxRange();
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(0f);
        enemy.atsm.attackState = this;
        isAnimationFinished = false;
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

    public virtual void TriggerAttack()
    {

    }

    public virtual void FinishAttack()
    {
        isAnimationFinished = true;
    }
}
