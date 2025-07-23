using UnityEngine;

public class UnlockDash : MonoBehaviour
{
    private GameManager _manager;

    private void Start() {
        _manager = Singleton.GameManager_Instance.Get<GameManager>();
    }

    public void Unlock() {
        _manager.Unlock_PlayerAbility(0);
    }
}
