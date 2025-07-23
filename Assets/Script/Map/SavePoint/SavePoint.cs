using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public enum SP_type { Main, Semi };
    [Header("type option")]
    public SP_type SavePoint_type;
    [Header("ID")]
    public int ID;
    [Space]
    [Header("is activate")]
    public bool SavePointEnabled = false;

    GameDataManager GameDataManager;

    private void Start()
    {
        GameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();
        SavePointEnabled = GameDataManager.SpawnPoints[ID];
    }

    public void InteractSavePoint()
    {
        print("SavePoint_" + SavePoint_type + " ID:" + ID);
        GameDataManager.SpawnPoint = ID;
        Singleton.GameManager_Instance.Get<GameManager>().SaveData__SavePoint();
    }
}