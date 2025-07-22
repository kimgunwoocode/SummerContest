using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public SaveData GameData { get; private set; }// 이전에 세이브된 데이터 임시 보관. 게임오버 또는 종료시에 다시 불러올 용도 (외부 접근 불가)

    //실제 게임 플레이 중에 접근할 변수들 :
    // Player Data
    public int MaxHP;
    public int CurrentHP;
    public int ATK;
    public float MaxBreathGauge;
    public float CurrentBreathGauge;
    public int Money = 0;
    public List<int> EquipSkill = new();

    public List<bool> PlayerAbility = new();
    public Dictionary<int, bool> PlayerSkill = new();
    public Dictionary<int, int> GettedItems = new();

    // Map Data
    public Dictionary<int, bool> InteractionObjects = new();
    public List<ShopData> Shops = new();
    public Dictionary<int, bool> SpawnPoints = new();
    public int SpawnPoint = -1;

    //Item Data
    public List<ItemData> allitems = new();


    public void LoadGameData(SaveData Data)
    {
        GameData = Data;
    }
}