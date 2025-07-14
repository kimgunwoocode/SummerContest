using UnityEngine;

//MonoBehaviour로
public class State
{
     protected FiniteStateMachine stateMachine;
     protected EnemyEntity enemy;

     protected float startTime;

     protected string animBoolName;

     public State(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName)
     {
          this.enemy = enemy;
          this.stateMachine = stateMachine;
          this.animBoolName = animBoolName;
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
