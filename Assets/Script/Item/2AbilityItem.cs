using UnityEngine;

[CreateAssetMenu(menuName = "Item/AbilityItem")]
public class AbilityItemData : ItemData, IAbilityItem
{
    [Header("플레이어 기능 슬롯 숫자")]
    [Tooltip("ID. 해금되는 기능\r\n 0. 대쉬\r\n 1. 브레스\r\n 2. 이단 점프\r\n 3. 낙하공격\r\n 4. 활공\r\n 5. 벽타기")]
    public int AbilitySlot;
    public void UnlockAbility()
    {
        Debug.Log($"{itemName} ({itemID}) 해금");
    }
}

public interface IAbilityItem
{
    void UnlockAbility();
}
