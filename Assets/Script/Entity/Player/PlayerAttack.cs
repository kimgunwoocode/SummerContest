using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    private ScriptablePlayerAttackStats _attackStats;
    private void Awake() {
        _attackStats = GetComponent<PlayerManager>().playerAttackStats;
    }


    private Vector3 Debugging;
    [SerializeField] ScriptablePlayerAttackStats test;

    internal void MeleeAttack(Vector3 direction) {
        Vector3 attackPos = transform.position + direction.normalized * _attackStats.MelleAttackRange * 0.5f;
        Debugging = attackPos;
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPos, _attackStats.MelleAttackRange, _attackStats.EnemyLayer); 
        foreach (var hit in hits) {
            Debug.Log(hit.name);
            hit.GetComponentInParent<EnemyEntity>()?.TakeDamage(_attackStats.MeleeAttackDamage, transform.position);
        }
    }
    private void OnDrawGizmosSelected() {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Debugging ,_attackStats.MelleAttackRange);
    }

    internal void NormalBreath() {

    }

    internal void ChangeBreath(int breathType) {

    }
}
