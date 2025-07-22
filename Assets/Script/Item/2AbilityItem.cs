using UnityEngine;

[CreateAssetMenu(menuName = "Item/AbilityItem")]
public class AbilityItemData : ItemData, IAbilityItem
{
    [Header("�÷��̾� ��� ���� ����")]
    [Tooltip("ID. �رݵǴ� ���\r\n 0. �뽬\r\n 1. �극��\r\n 2. �̴� ����\r\n 3. ���ϰ���\r\n 4. Ȱ��\r\n 5. ��Ÿ��")]
    public int AbilitySlot;
    public void UnlockAbility()
    {
        Debug.Log($"{itemName} ({itemID}) �ر�");
    }
}

public interface IAbilityItem
{
    void UnlockAbility();
}
