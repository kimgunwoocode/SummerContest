using UnityEngine;

[CreateAssetMenu(menuName = "Item/NomalItem")]
public class NomalItemData : ItemData, INoamlItem
{
    public bool isStackable;     // �������� ���� �� ���� �� �ִ°�?
    public int maxStackCount;    // �ִ� ��ø ����
}
public interface INoamlItem
{

}
