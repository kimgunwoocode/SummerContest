using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyData", menuName = "Data/Enemy Data/Base Data")]

public class D_Enemy : ScriptableObject
{
   public int maxHP;

   public float wallCheckDistance = .2f;
   public float ledgeCheckDistance = .4f;
   public float groundCkeckRadius = .3f;

   public float minPlayerCheckDistance = 3f;
   public float maxPlayerCheckDistance = 4f;

   //public int stunResistance = 3;
   //public float stunRecoveryTime = 2f;

   public float closeRangeActionDistance = 1f;

   public GameObject hitParticle;

   public LayerMask groundLayer;
   public LayerMask playerLayer;
}
