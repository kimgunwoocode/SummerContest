using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum ItemType { Nomal, Skill, Breath }//Nomal : 설명만 있는 수집요소 아이템, Skill : 기능해금을 나타내는 아이템, Breath : 커스텀공격을 위한 아이템

[CreateAssetMenu(menuName = "Item/ItemData")]
public abstract class ItemData : ScriptableObject
{
    public int itemID; //Nomal : 1000 ~ 1999, Skill : 2000 ~ 2999, Breath : 3000 ~ 3999
    public string itemName;
    public Sprite icon;
    public string description;
    public ItemType itemType;
    public abstract void OnGetItem();
}


public class MapData
{
    public List<GameObject> InteractionObjects = new();
    public int SpawnPoint = -1;
}

public class PlayerData
{
    public Dictionary<string, bool> PlayerSkill = new();
}

public class SaveData
{
    public List<bool> isObjectsInteracted = new();
    public int SpawnPoint = -1;

    public int money = 0;
}
