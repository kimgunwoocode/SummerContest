using UnityEngine;

[CreateAssetMenu(menuName = "Item/NomalItem")]
public class NomalItemData : ItemData, INoamlItem
{
    public bool isStackable;     // 아이템이 여러 개 쌓일 수 있는가?
    public int maxStackCount;    // 최대 중첩 개수
}
public interface INoamlItem
{

}
