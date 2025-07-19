using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int MaxHP; // 플레이어 최대 체력
    public int CurrentHP; // 플레이어 현재 체력
    public int ATK;
    public float MaxBreathGauge; // 최대 브레스 게이지
    public float CurrentBreathGauge; // 브레스 게이지
    public int Money; // 보유중인 돈
    public List<int> EquipSkill; // 장착중인 스킬


    public Dictionary<int, bool> PlayerSkill = new(); // 해금된 플레이어 스킬 <스킬ID, 해금 여부>
    public Dictionary<int, bool> GettedItems = new(); // 보유중인 아이템
}