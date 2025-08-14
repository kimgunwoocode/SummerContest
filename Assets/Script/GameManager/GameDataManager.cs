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

    [Space]
    [Header("브레스 게이지 회복 속도 설정")]
    // 저장하는 변수 X --> 그냥 내부 시스템으로 고정할거임. 필요하면 그 때 아이템 정보 가져와서 따로 계산하기
    public float BreathFillSpeed = 0.1f;
    [Space]

    public List<bool> PlayerAbility = new();
    public Dictionary<int, bool> PlayerSkill = new();
    public Dictionary<int, int> GettedItems = new();

    // Map Data
    public Dictionary<int, bool> InteractionObjects = new();
    public Dictionary<int, Vector2> PushObjects = new();
    public List<ShopData> Shops = new();
    public Dictionary<int, bool> SpawnPoints = new();
    public int SpawnPoint = -1;


    //Item Data
    public AllItems allitems_SO;
    public Dictionary<int, ItemData> allitems = new();

    private void Awake()
    {
        Apply_allitems();
    }
    private void Start()
    {
        /*
        // 세이브파일에서 장착중인 브레스 스킬 정보를 실제 데이터에 적용하기
        foreach(int ID in EquipSkill)
        {
            EquipBreathItemData.Add(allitems_dic[ID] as BreathItemData);
        }
        */
    }
    public void LoadGameData(SaveData Data)
    {
        GameData = Data;
    }

    public void NowInvisible(int ID){
        InteractionObjects[ID] = false;
    }

    public void Apply_allitems()
    {
        foreach (var kv in allitems_SO.allitems)
        {
            int id = kv.itemID;

            allitems[id] = kv;
        }
    }
}