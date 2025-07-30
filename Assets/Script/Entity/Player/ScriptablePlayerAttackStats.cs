using UnityEngine;

[CreateAssetMenu]
public class ScriptablePlayerAttackStats : ScriptableObject {
    public LayerMask EnemyLayer;
    public float MeleeAttackDuration = 0.1f;
    public int MeleeAttackDamage = 1;
    public float MeleeAttackCooldown = 0.2f;
    public float MeleeAttackRange = 2f;

    public float BreathReadyTime = 2f;
}
