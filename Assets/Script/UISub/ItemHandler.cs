using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    private GameManager _gameManager;
    private GameDataManager _gameDataManager;

    private void Awake()
    {
        _gameManager = Singleton.GameManager_Instance.Get<GameManager>();
        _gameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();
    }

    public void CallGetItem(int id) {
        _gameManager.Get_Item(id);
    }

    public void CallNowInvisible(int id)
    {
        _gameDataManager.NowInvisible(id);
    }
}