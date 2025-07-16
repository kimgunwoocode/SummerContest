using UnityEngine;

public abstract class State : MonoBehaviour
{
     protected FiniteStateMachine stateMachine;
     protected EnemyEntity enemy;
     protected float startTime;
     protected string animBoolName;

     public virtual void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
     {
          this.enemy = enemy;
          this.stateMachine = stateMachine;
     }

     public virtual void Enter()
     {
          startTime = Time.time;
          enemy.anim.SetBool(animBoolName, true);
          DoCheck();
     }

     public virtual void Exit()
     {
          enemy.anim.SetBool(animBoolName, false);
     }

     public virtual void LogicUpdate()
     {

     }

     public virtual void PhysicsUpdate()
     {
          DoCheck();
     }

     public virtual void DoCheck()
     {

     }
}
