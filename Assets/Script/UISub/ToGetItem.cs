using UnityEngine;

public class ToGetItem : MonoBehaviour
{
    GameManager GameManager;
    GameDataManager GameDataManager;

    private void Awake()
    {
        GameManager = Singleton.GameManager_Instance.Get<GameManager>();
        GameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();
    }

    public void CallGetItem(int ID) {
        GameManager.Get_Item(ID);
    }

    public void CallNowInvisible(int ID)
    {
        GameDataManager.NowInvisible(ID);
    }
}
