using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    private ScriptablePlayerAttackStats _attackStats;
    private int _currentBreath;

    private void Awake() {
        _attackStats = GetComponent<PlayerManager>().playerAttackStats;
    }


    private float _lastAttackTime = 0f;

    internal void MeleeAttack(Vector3 direction) {
        if (Time.time - _lastAttackTime < _attackStats.MeleeAttackCooldown) {
            Debug.Log("Melee Attack is on Cooldown");
            return;
        }

        Vector3 attackPos = transform.position + direction.normalized * _attackStats.MeleeAttackRange * 0.5f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPos, _attackStats.MeleeAttackRange, _attackStats.EnemyLayer); 
        foreach (var hit in hits) {
            Debug.Log(hit.name);
            hit.GetComponentInParent<EnemyEntity>()?.TakeDamage(_attackStats.MeleeAttackDamage, transform.position);
        }
        _lastAttackTime = Time.time;
    }

    internal void FireBreath() {
        
    }

    internal void ChangeBreath(int breathType) {

    }

    #region Debugging
    [SerializeField] ScriptablePlayerAttackStats debug;
    private void OnDrawGizmos() {
        if (transform == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position ,debug.MeleeAttackRange);
    }
    #endregion
}
