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
    [Space]
    public Interaction interaction;
    GameDataManager GameDataManager;

    private void Start()
    {
        if (interaction == null) interaction = GetComponent<Interaction>();
        GameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();
        if (GameDataManager.SpawnPoints?.Count > 0)
            SavePointEnabled = GameDataManager.SpawnPoints[ID];
    }

    public void InteractSavePoint()
    {
        if (interaction.isInteracted)
        {
            interaction.isInteracted = false;
            GameDataManager.InteractionObjects[ID] = false;
            GameDataManager.SpawnPoints[ID] = true;
            // 활성 애니메이션
        }
        print("SavePoint_" + SavePoint_type + " ID:" + ID);
        GameDataManager.SpawnPoint = ID;
        Singleton.GameManager_Instance.Get<GameManager>().SaveData__SavePoint();
    }

    // 한 번 상호작용했으면 활성화 시키기
    public void InitInteractedSavePoint()
    {
        SavePointEnabled = true;
        // 활성화 상태로 전환 (스프라이트 등...)
    }
}