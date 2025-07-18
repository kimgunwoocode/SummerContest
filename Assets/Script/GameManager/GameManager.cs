using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameDataManager GameDataManager;
    public MapDataManager MapDataManager;
    public GameObject Player;
    public string CurrentSceneName;

    private void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        if (GameDataManager == null)
        {
            GameDataManager = gameObject.GetComponent<GameDataManager>();
        }
        if (MapDataManager == null)
        {
            MapDataManager = gameObject.GetComponent<MapDataManager>();
        }
    }






    public void PlayerDie()//플레이어 사망시 호출해야할 함수
    {
        LoadData__SavePoint();//이전 세이브 포인트로 시점 되돌리기

        //이전 세이브 포인트로 위치 이동시키기
    }






    public void StartGame_LoadData_from_SaveFile(PlayerData playerData)
    {

        LoadData__SavePoint();
    }

    private void LoadData__SavePoint()
    {
        //플레이어 데이터
        GameDataManager.MaxHP = GameDataManager.GameManager_PlayerData.MaxHP;
        GameDataManager.CurrentHP = GameDataManager.GameManager_PlayerData.CurrentHP;
        GameDataManager.ATK = GameDataManager.GameManager_PlayerData.ATK;
        GameDataManager.MaxBreathGauge = GameDataManager.GameManager_PlayerData.MaxBreathGauge;
        GameDataManager.CurrentBreathGauge = GameDataManager.GameManager_PlayerData.CurrentBreathGauge;
        GameDataManager.Money = GameDataManager.GameManager_PlayerData.Money;
        GameDataManager.EquipSkill = GameDataManager.GameManager_PlayerData.EquipSkill;
        GameDataManager.PlayerSkill = GameDataManager.GameManager_PlayerData.PlayerSkill;
        GameDataManager.GettedItems = GameDataManager.GameManager_PlayerData.GettedItems;

        //맵 데이터
        MapDataManager.InteractionObjects = MapDataManager.GameManager_MapData.InteractionObjects;
        MapDataManager.Shops = MapDataManager.GameManager_MapData.Shops;
        MapDataManager.SpawnPoints = MapDataManager.GameManager_MapData.SpawnPoints;
        MapDataManager.SpawnPoint = MapDataManager.GameManager_MapData.SpawnPoint;
    }
    public void SaveData__SavePoint()
    {
        //플레이어 데이터
        GameDataManager.GameManager_PlayerData.MaxHP = GameDataManager.MaxHP;
        GameDataManager.GameManager_PlayerData.CurrentHP = GameDataManager.CurrentHP;
        GameDataManager.GameManager_PlayerData.ATK = GameDataManager.ATK;
        GameDataManager.GameManager_PlayerData.MaxBreathGauge = GameDataManager.MaxBreathGauge;
        GameDataManager.GameManager_PlayerData.CurrentBreathGauge = GameDataManager.CurrentBreathGauge;
        GameDataManager.GameManager_PlayerData.Money = GameDataManager.Money;
        GameDataManager.GameManager_PlayerData.EquipSkill = GameDataManager.EquipSkill;
        GameDataManager.GameManager_PlayerData.PlayerSkill = GameDataManager.PlayerSkill;
        GameDataManager.GameManager_PlayerData.GettedItems = GameDataManager.GettedItems;

        //맵 데이터
        MapDataManager.GameManager_MapData.InteractionObjects = MapDataManager.InteractionObjects;
        MapDataManager.GameManager_MapData.Shops = MapDataManager.Shops;
        MapDataManager.GameManager_MapData.SpawnPoints = MapDataManager.SpawnPoints;
        MapDataManager.GameManager_MapData.SpawnPoint = MapDataManager.SpawnPoint;



        //세이브파일에 저장하는 프로세스도 여기에 만들어두기
    }
}
