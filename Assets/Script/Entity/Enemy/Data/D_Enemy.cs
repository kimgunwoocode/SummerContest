using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyData", menuName = "Data/Enemy Data/Base Data")]

public class D_Enemy : ScriptableObject
{
   public string enemyName;
   public string enemyInfo;

   [Tooltip("적의 최대 체력")]
   public int maxHP;

   [Tooltip("벽 체크 거리")]
   public float wallCheckDistance = .2f;
   [Tooltip("절벽 체크 거리")]
   public float ledgeCheckDistance = .4f;
   [Tooltip("땅 체크 반지름")]
   public float groundCkeckRadius = .3f;

   [Tooltip("플레이어 발견 거리")]
   public float minPlayerCheckDistance = 3f;
   [Tooltip("플레이어 발견 해제 거리")]
   public float maxPlayerCheckDistance = 4f;
   [Tooltip("근접 공격 거리")]
   public float closeRangeActionDistance = 1f;

   public GameObject hitParticle;

   public LayerMask groundLayer;
   public LayerMask playerLayer;

   //public int stunResistance = 3;
   //public float stunRecoveryTime = 2f;
}
