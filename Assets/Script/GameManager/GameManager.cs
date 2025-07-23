using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameDataManager GameDataManager;
    public GameObject Player;
    public string CurrentSceneName;

    private void Awake()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    private void Start()
    {
        if (GameDataManager == null)
        {
            GameDataManager = gameObject.GetComponent<GameDataManager>();
        }
    }






    public void PlayerDie()//플레이어 사망시 호출해야할 함수
    {
        LoadData__SavePoint();//이전 세이브 포인트로 시점 되돌리기

        //이전 세이브 포인트로 위치 이동시키기
    }

    public void Get_Item(int ItemID)
    {
        GameDataManager.GettedItems[ItemID]++;

        ItemData item = GameDataManager.allitems.Find(item => item.itemID == ItemID);
        if (item == null)
        {
            Debug.LogWarning("존재하지 않는 아이템 ID");
            return;
        }

        // 능력해금 아이템일 경우 획득시 능력 해금하기
        if (item.itemType == ItemType.Ability && item is AbilityItemData abilityItem) 
        {
            int slot = abilityItem.AbilitySlot;
            abilityItem.UnlockAbility();
            Unlock_PlayerAbility(slot);
        }
    }

    public void Lose_Item(int ItemID)
    {
        if (GameDataManager.GettedItems[ItemID] > 0)
            GameDataManager.GettedItems[ItemID]--;
    }

    public void Unlock_PlayerAbility(int PlayerAbilityID)
    {
        //TODO : PlayerAbility 작성하기
        if (GameDataManager.PlayerAbility.Count != 0)
            GameDataManager.PlayerAbility[PlayerAbilityID] = true;

        else {
            GameDataManager.PlayerAbility = new List<bool>() {false, false, false, false, false, false};
            GameDataManager.PlayerAbility[PlayerAbilityID] = true;
        }

        // 플레이어에서 기능 해금 이벤트 호출하기
        Player.GetComponent<PlayerManager>().UnlockAbility(PlayerAbilityID);
    }






    public void StartGame_LoadData_from_SaveFile(PlayerData playerData)
    {

        LoadData__SavePoint();
    }

    public void LoadData__SavePoint()
    {
        //플레이어 데이터
        GameDataManager.MaxHP = GameDataManager.GameData.PlayerData.MaxHP;
        GameDataManager.CurrentHP = GameDataManager.GameData.PlayerData.CurrentHP;
        GameDataManager.ATK = GameDataManager.GameData.PlayerData.ATK;
        GameDataManager.MaxBreathGauge = GameDataManager.GameData.PlayerData.MaxBreathGauge;
        GameDataManager.CurrentBreathGauge = GameDataManager.GameData.PlayerData.CurrentBreathGauge;
        GameDataManager.Money = GameDataManager.GameData.PlayerData.Money;
        GameDataManager.EquipSkill = GameDataManager.GameData.PlayerData.EquipSkill;
        GameDataManager.PlayerAbility = GameDataManager.GameData.PlayerData.PlayerAbility;
        GameDataManager.PlayerSkill = GameDataManager.GameData.PlayerData.PlayerSkill;
        GameDataManager.GettedItems = GameDataManager.GameData.PlayerData.GettedItems;

        //맵 데이터
        GameDataManager.InteractionObjects = GameDataManager.GameData.MapData.InteractionObjects;
        GameDataManager.PushObjects = GameDataManager.GameData.MapData.PushObjects;
        GameDataManager.Shops = GameDataManager.GameData.MapData.Shops;
        GameDataManager.SpawnPoints = GameDataManager.GameData.MapData.SpawnPoints;
        GameDataManager.SpawnPoint = GameDataManager.GameData.MapData.SpawnPoint;

        //데이터를 맵 요소에 적용시키기
    }
    public void SaveData__SavePoint()
    {
        //플레이어 데이터
        GameDataManager.GameData.PlayerData.MaxHP = GameDataManager.MaxHP;
        GameDataManager.GameData.PlayerData.CurrentHP = GameDataManager.CurrentHP;
        GameDataManager.GameData.PlayerData.ATK = GameDataManager.ATK;
        GameDataManager.GameData.PlayerData.MaxBreathGauge = GameDataManager.MaxBreathGauge;
        GameDataManager.GameData.PlayerData.CurrentBreathGauge = GameDataManager.CurrentBreathGauge;
        GameDataManager.GameData.PlayerData.Money = GameDataManager.Money;
        GameDataManager.GameData.PlayerData.EquipSkill = GameDataManager.EquipSkill;
        GameDataManager.GameData.PlayerData.PlayerAbility = GameDataManager.PlayerAbility;
        GameDataManager.GameData.PlayerData.PlayerSkill = GameDataManager.PlayerSkill;
        GameDataManager.GameData.PlayerData.GettedItems = GameDataManager.GettedItems;

        //맵 데이터
        GameDataManager.GameData.MapData.InteractionObjects = GameDataManager.InteractionObjects;
        GameDataManager.GameData.MapData.PushObjects = GameDataManager.PushObjects;
        GameDataManager.GameData.MapData.Shops = GameDataManager.Shops;
        GameDataManager.GameData.MapData.SpawnPoints = GameDataManager.SpawnPoints;
        GameDataManager.GameData.MapData.SpawnPoint = GameDataManager.SpawnPoint;



        //세이브파일에 저장하는 프로세스도 여기에 만들어두기
        SaveFileManager.Save(GameDataManager.GameData, GameDataManager.GameData.Slot);
    }
}