using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum ItemType { Nomal, Skill, Breath }//Nomal : 설명만 있는 수집요소 아이템, Skill : 기능해금을 나타내는 아이템, Breath : 커스텀공격을 위한 아이템

[CreateAssetMenu(menuName = "Item/ItemData")]
public abstract class ItemData : ScriptableObject
{
    public ItemType itemType;
    public int itemID; //Nomal: 1000 ~ 1999, Skill: 2000 ~ 2999, Breath: 3000 ~ 3999
    public Sprite icon;
    public string itemName;
    public string description;

    public int price;           //아이템 구매 또는 판매할 때 금액. 브레스일 경우는 브레스 게이지 사용량
    public bool isStackable;     // 아이템이 여러 개 쌓일 수 있는가?
    public int maxStackCount;    // 최대 중첩 개수

    public abstract void OnGetItem();
    public abstract void OnUseItem();
}
