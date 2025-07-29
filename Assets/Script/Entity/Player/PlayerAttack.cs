using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    private ScriptablePlayerAttackStats _attackStats;
    private PlayerManager _pm;
    private GameDataManager _data;
    private int _currentBreathId;
    private BreathItemData _currentBreathInfo;

    private void Awake() {
        _data = Singleton.GameManager_Instance.Get<GameDataManager>();
        _attackStats = GetComponent<PlayerManager>().playerAttackStats;
        _pm = GetComponent<PlayerManager>();
    }

    internal void InitiateBreath() {
        _currentBreathId = _data.EquipSkill.Count == 0 ? -1 : _data.EquipSkill[0];
        _currentBreathInfo = _currentBreathId == -1 ? basic :_data.allitems[_currentBreathId] as BreathItemData;
        if (_currentBreathInfo == null) Debug.LogError("error");
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

    internal void FireBreath(Vector3 direction) {
        _currentBreathInfo.UseBreath(direction, transform.position);
    }

    internal void ChangeBreath(int breathType) {

    }

    #region Debugging
    [SerializeField] ScriptablePlayerAttackStats debug;
    [SerializeField] BreathItemData basic;
    private void OnDrawGizmos() {
        if (transform == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position ,debug.MeleeAttackRange);
    }
    #endregion
}
