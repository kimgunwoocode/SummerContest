using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public enum SP_type { Main, Semi };
    [Header("type option")]
    public SP_type SavePoint_type;
    [Header("ID")]
    public int ID;
    public string SceneName;
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
        if (interaction.isInteracted)// 상호작용 가능할 때
        {
            interaction.isInteracted = false;// 상호작용 했으므로 상호작용 비활성화 시키기
            GameDataManager.InteractionObjects[ID] = false;// 데이터의 상호작용 오브젝트 값 변경
            GameDataManager.SpawnPoints[ID] = true;// 데이터의 세이브포인트 값 변경
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