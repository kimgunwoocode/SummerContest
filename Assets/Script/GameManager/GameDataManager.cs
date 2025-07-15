using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int MaxHP; // 플레이어 최대 체력
    public int CurrentHP; // 플레이어 현재 체력
    public float MaxBreathGauge; // 최대 브레스 게이지
    public float CurrentBreathGauge; // 브레스 게이지
    public int Money; // 보유중인 돈
    public Dictionary<int, bool> PlayerSkill = new();// 해금된 플레이어 스킬 <스킬ID, 해금 여부>
    public Dictionary<int, bool> GettedItems = new(); // 보유중인 아이템
}

public class GameDataManager : MonoBehaviour
{
    public PlayerData GameManager_PlayerData { get; private set; }// 이전에 세이브된 데이터 임시 보관. 게임오버 또는 종료시에 다시 불러올 용도 (외부 접근 불가)

    //실제 게임 플레이 중에 접근할 변수들 :
    public int MaxHP;
    public int CurrentHP;
    public float MaxBreathGauge;
    public float CurrentBreathGauge;
    public int Money = 0;
    public Dictionary<int, bool> PlayerSkill = new();
    public Dictionary<int, bool> GettedItems = new();


    public void StartGame_LoadData_from_SaveFile(PlayerData playerData)
    {

        LoadData_from_SavePoint();
    }

    public void LoadData_from_SavePoint()
    {
        MaxHP = GameManager_PlayerData.MaxHP;
        CurrentHP = GameManager_PlayerData.CurrentHP;
        MaxBreathGauge = GameManager_PlayerData.MaxBreathGauge;
        CurrentBreathGauge = GameManager_PlayerData.CurrentBreathGauge;
        Money = GameManager_PlayerData.Money;
        PlayerSkill = GameManager_PlayerData.PlayerSkill;
        GettedItems = GameManager_PlayerData.GettedItems;
    }
    public void SaveData_to_SavePoint()
    {
        GameManager_PlayerData.MaxHP = MaxHP;
        GameManager_PlayerData.CurrentHP = CurrentHP;
        GameManager_PlayerData.MaxBreathGauge = MaxBreathGauge;
        GameManager_PlayerData.CurrentBreathGauge = CurrentBreathGauge;
        GameManager_PlayerData.Money = Money;
        GameManager_PlayerData.PlayerSkill = PlayerSkill;
        GameManager_PlayerData.GettedItems = GettedItems;

        //세이브파일에 저장하는 프로세스도 여기에 만들어두기
    }
}
