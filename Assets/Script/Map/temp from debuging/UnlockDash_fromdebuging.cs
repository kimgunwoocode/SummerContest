using UnityEngine;

public class UnlockDash_fromdebuging : MonoBehaviour
{
    private GameManager _manager;
    private GameDataManager _gameDataManager;
    public Interaction interaction;

    private void Start() {
        _manager = Singleton.GameManager_Instance.Get<GameManager>();
        _gameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();
    }

    public void Unlock() {
        _manager.Get_Item(1500);
        Destroygameobject();
        _gameDataManager.InteractionObjects[interaction.ID] = false;
    }

    public void Destroygameobject()
    {
        Destroy(gameObject);
    }
}
