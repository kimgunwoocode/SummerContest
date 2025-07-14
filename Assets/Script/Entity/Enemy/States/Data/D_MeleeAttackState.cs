using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/State Data/MeleeAttack State")]
public class D_MeleeAttackState : ScriptableObject
{
   public float attackRadius = .5f;
   public int attackDamage = 1;
   public LayerMask playerLayer;
}
