using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum ItemType { Nomal, Ability, Breath }//Nomal : 설명만 있는 수집요소 아이템, Ability : 기능해금을 나타내는 아이템, Breath : 커스텀공격을 위한 아이템

public abstract class ItemData : ScriptableObject
{
    public ItemType itemType;
    public int itemID;
    public Sprite icon;
    public string itemName;
    [TextArea]
    public string description;

    public int price;           //아이템 구매 또는 판매할 때 금액
}
