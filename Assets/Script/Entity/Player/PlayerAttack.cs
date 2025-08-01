using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
    [SerializeField] private GameObject MeleeEffect;
    private ScriptablePlayerAttackStats _attackStats;
    private PlayerManager _pm;
    private GameDataManager _data;
    private int _currentBreathId;
    private BreathItemData _currentBreathInfo;

    private float MaxBreathGauge;
    private float CurrentBreathGauge;

    private void Awake() {
        _data = Singleton.GameManager_Instance.Get<GameDataManager>();
        _attackStats = GetComponent<PlayerManager>().playerAttackStats;
        _pm = GetComponent<PlayerManager>();
    }

    internal void InitiateBreath() {
        //MaxBreathGauge = _data.MaxBreathGauge == 0 ? 100 : _data.MaxBreathGauge;
        //CurrentBreathGauge = MaxBreathGauge;

        _currentBreathId = _data.EquipSkill.Count == 0 ? -1 : _data.EquipSkill[0];
        _currentBreathInfo = _currentBreathId == -1 ? basic :_data.allitems[_currentBreathId] as BreathItemData;
        if (_currentBreathInfo == null) Debug.LogError("error");
    }


    private float _lastAttackTime = 0f;
    private float _lastBreathFireTime = 0f;

    internal void MeleeAttack(Vector3 direction) {
        if (Time.time - _lastAttackTime < _attackStats.MeleeAttackCooldown) {
            return;
        }

        Vector3 attackPos = transform.position + direction.normalized * _attackStats.MeleeAttackRange * 2f;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Instantiate(MeleeEffect, attackPos, Quaternion.Euler(0, 0, angle - 90));
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _attackStats.MeleeAttackRange, _attackStats.EnemyLayer); 
        foreach (var hit in hits) {
            hit.GetComponentInParent<EnemyEntity>()?.TakeDamage(_data.ATK, transform.position);
            _data.CurrentBreathGauge += 1;
        }
        _lastAttackTime = Time.time;
    }

    internal void FireBreath(Vector3 direction) {
        if (Time.time - _lastBreathFireTime < _currentBreathInfo.breathCoolDown || _data.CurrentBreathGauge < _currentBreathInfo.breathCost) {
            return;
        }

        _data.CurrentBreathGauge -= _currentBreathInfo.breathCost;
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
