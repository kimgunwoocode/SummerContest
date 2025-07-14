using UnityEngine;

public class AnimationToStatemachine : MonoBehaviour
{
    public AttackState attackState;

   void TriggerAttack()
   {
        attackState.TriggerAttack();
   }

   void FinishAttack()
   {
        attackState.FinishAttack();
   }
}