using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameDataManager GameDataManager;
    public TextAsset initdata;
    public GameObject Player;

    [Header("씬 이동 시 가져가야할 정보들")]
    //public string CurrentSceneName;
    public int CurrentScenePointID = -1;
    public int CurrentStartSceneCameraArea = 0;




    public TextAsset SavePointID_json;
    Dictionary<int, string> SavePointID_list = new();



    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log(scene.name);
        if (scene.name == "Title")
            return;
        if (Player == null || Player.activeSelf == false)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }



    private void Awake()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        /*
        if (CurrentSceneName == null)
        {
            CurrentSceneName = SceneManager.GetActiveScene().name;
        }
        */
        SavePointID_list = DictionaryFromJson(SavePointID_json.text);
    }
    private void Start()
    {
        if (GameDataManager == null)
        {
            GameDataManager = gameObject.GetComponent<GameDataManager>();
        }
    }


    private void Update()
    {
        FillBreathGauge_byUpdate();// 브레스 게이지 회복
    }





    private void FillBreathGauge_byUpdate()
    {
        if (GameDataManager.CurrentBreathGauge < GameDataManager.MaxBreathGauge)
        {
            GameDataManager.CurrentBreathGauge += GameDataManager.BreathFillSpeed * Time.deltaTime;
        }
        else if (GameDataManager.CurrentBreathGauge > GameDataManager.MaxBreathGauge)
            GameDataManager.CurrentBreathGauge = GameDataManager.MaxBreathGauge;
    }


    public void PlayerDie()//플레이어 사망시 호출해야할 함수
    {
        LoadData__SavePoint();//이전 세이브 포인트로 시점 되돌리기


        //이전 세이브 포인트로 위치 이동시키기
        if (GameDataManager.SpawnPoint == -1)
        {
            SceneManager.LoadScene("1-1_ForgottenNest");
        }
        string SavedSceneName = SavePointID_list[GameDataManager.SpawnPoint];
        CurrentScenePointID = -GameDataManager.SpawnPoint;
        SceneManager.LoadScene(SavedSceneName);
    }

    public void Get_Money(int min, int max)
    {
        GameDataManager.Money += Random.Range(min, max);
    }

    public void Get_Item(int ItemID)
    {
        GameDataManager.GettedItems[ItemID]++;

        ItemData item = GameDataManager.allitems[ItemID];
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

        else
        {
            GameDataManager.PlayerAbility = new List<bool>() { false, false, false, false, false, false };
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


    public Dictionary<int, string> DictionaryFromJson(string SavePointID_json)
    {
        SavePointID_Wrapper savepointID_wrapper = JsonUtility.FromJson<SavePointID_Wrapper>(SavePointID_json);
        Dictionary<int, string> savepointID_list = new();
        foreach (var sp in savepointID_wrapper.savepoint_list)
        {
            savepointID_list[sp.ID] = sp.ScenName;
        }
        return savepointID_list;

    }

    [System.Serializable]
    private class SavePointID_Wrapper
    {
        public List<SavePoint_class> savepoint_list;
    }
    [System.Serializable]
    private class SavePoint_class
    {
        public int ID;
        public string ScenName;
    }
}