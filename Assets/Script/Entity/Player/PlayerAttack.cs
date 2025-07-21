using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    private ScriptablePlayerAttackStats _attackStats;
    private void Awake() {
        _attackStats = GetComponent<PlayerManager>().playerAttackStats;
    }

    internal void NormalAttack() {

    }

    internal void NormalBreath() {

    }

    internal void ChangeBreath(int breathType) {

    }
}
